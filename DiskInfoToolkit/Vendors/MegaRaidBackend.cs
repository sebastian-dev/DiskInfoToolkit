/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

using DiskInfoToolkit.Constants;
using DiskInfoToolkit.Core;
using DiskInfoToolkit.Interop;
using DiskInfoToolkit.Native;
using DiskInfoToolkit.Probes;
using DiskInfoToolkit.Utilities;
using System.Runtime.InteropServices;
using System.Text;

namespace DiskInfoToolkit.Vendors
{
    public sealed class MegaRaidBackend : IOptionalVendorBackend
    {
        #region Constructor

        public MegaRaidBackend(ExternalVendorLibraryManager libraries)
        {
            _libraries = libraries;
            _capabilities = new MegaRaidBackendCapabilities();
        }

        #endregion

        #region Fields

        private readonly ExternalVendorLibraryManager _libraries;

        private bool _exportsResolved;

        private MegaRaidBackendCapabilities _capabilities;

        private ProcessLibCommandDelegate _processLibCommand;

        #endregion

        #region Properties

        public bool IsAvailable
        {
            get
            {
                var handle = _libraries.GetMegaRaidLibrary();
                return handle != null && !handle.IsInvalid;
            }
        }

        public MegaRaidBackendCapabilities Capabilities
        {
            get
            {
                EnsureExportsResolved();
                return _capabilities;
            }
        }

        #endregion

        #region Public

        public bool TryProbe(StorageDevice device)
        {
            if (device == null)
            {
                return false;
            }

            var handle = _libraries.GetMegaRaidLibrary();
            if (handle == null || handle.IsInvalid)
            {
                device.ProbeTrace.Add("Vendor backend: MegaRAID library is not available.");
                return false;
            }

            EnsureExportsResolved();

            if (!_capabilities.HasCoreExports || _processLibCommand == null)
            {
                device.ProbeTrace.Add("Vendor backend: MegaRAID ProcessLibCommand export is not available.");
                return false;
            }

            device.ProbeTrace.Add("Vendor backend: MegaRAID ProcessLibCommand export resolved.");
            if (string.IsNullOrWhiteSpace(device.Controller.Kind))
            {
                device.Controller.Kind = StorageTextConstants.MegaRaid;
            }

            if (device.Controller.Family == StorageControllerFamily.Unknown)
            {
                device.Controller.Family = StorageControllerFamily.MegaRaid;
            }

            if (device.TransportKind == StorageTransportKind.Unknown)
            {
                device.TransportKind = StorageTransportKind.Raid;
            }

            if (!TryInitializeLibrary(out var controllerIds) || controllerIds.Length == 0)
            {
                device.ProbeTrace.Add("Vendor backend: MegaRAID library initialized but no controllers were reported.");
                return false;
            }

            device.ProbeTrace.Add($"Vendor backend: MegaRAID controllers reported={controllerIds.Length}.");

            bool success = false;
            for (int i = 0; i < controllerIds.Length; ++i)
            {
                uint controllerId = controllerIds[i];
                ProbeTraceRecorder.Add(device, $"Vendor backend: MegaRAID probing controller id {controllerId}.");

                if (TryGetControllerInfo(controllerId, out var controllerInfo))
                {
                    ApplyControllerInfoFromBuffer(device, controllerInfo, controllerId);
                    ProbeTraceRecorder.Add(device, $"Vendor backend: MegaRAID controller info retrieved for controller id {controllerId}.");
                    success = true;
                }

                if (!TryGetPhysicalDriveList(controllerId, out var physicalDriveIds) || physicalDriveIds.Length == 0)
                {
                    continue;
                }

                ProbeTraceRecorder.Add(device, $"Vendor backend: MegaRAID physical drives reported={physicalDriveIds.Length} on controller id {controllerId}.");

                byte preferredPhysicalDriveId = 0xFF;
                if (device.Scsi.TargetID.HasValue)
                {
                    preferredPhysicalDriveId = device.Scsi.TargetID.Value;
                }

                if (preferredPhysicalDriveId != 0xFF)
                {
                    for (int n = 0; n < physicalDriveIds.Length; ++n)
                    {
                        if (physicalDriveIds[n] == preferredPhysicalDriveId)
                        {
                            if (TryGetPhysicalDriveInfo(device, controllerId, preferredPhysicalDriveId))
                            {
                                success = true;
                                TryPassThroughPhysicalDrive(device, controllerId, preferredPhysicalDriveId);
                                break;
                            }
                        }
                    }
                }

                if (!HasUsefulDeviceIdentity(device))
                {
                    for (int n = 0; n < physicalDriveIds.Length; ++n)
                    {
                        if (TryGetPhysicalDriveInfo(device, controllerId, physicalDriveIds[n]))
                        {
                            success = true;
                            TryPassThroughPhysicalDrive(device, controllerId, physicalDriveIds[n]);
                            break;
                        }

                        if (TryPassThroughPhysicalDrive(device, controllerId, physicalDriveIds[n]))
                        {
                            success = true;
                            break;
                        }
                    }
                }

                if (success && HasUsefulDeviceIdentity(device))
                {
                    break;
                }
            }

            if (success)
            {
                if (string.IsNullOrWhiteSpace(device.Controller.Kind))
                {
                    device.Controller.Kind = StorageTextConstants.MegaRaid;
                }

                if (string.IsNullOrWhiteSpace(device.Controller.Name))
                {
                    device.Controller.Name = StorageTextConstants.MegaRaid;
                }
                return true;
            }

            device.ProbeTrace.Add("Vendor backend: MegaRAID command backend is available but no device data was obtained.");
            return false;
        }

        #endregion

        #region Private

        private static void WriteUInt16(byte[] buffer, int offset, ushort value)
        {
            var bytes = BitConverter.GetBytes(value);
            Buffer.BlockCopy(bytes, 0, buffer, offset, 2);
        }

        private static void WriteUInt32(byte[] buffer, int offset, uint value)
        {
            var bytes = BitConverter.GetBytes(value);
            Buffer.BlockCopy(bytes, 0, buffer, offset, 4);
        }

        private static List<string> ExtractAsciiTokens(byte[] data, int minimumLength, int maximumTokens)
        {
            var tokens = new List<string>();
            if (data == null || data.Length == 0)
            {
                return tokens;
            }

            var current = new StringBuilder();
            for (int i = 0; i < data.Length; ++i)
            {
                byte value = data[i];
                if (value >= 0x20 && value <= 0x7E)
                {
                    current.Append((char)value);
                }
                else
                {
                    AddAsciiToken(tokens, current, minimumLength, maximumTokens);
                    if (tokens.Count >= maximumTokens)
                    {
                        break;
                    }
                }
            }

            AddAsciiToken(tokens, current, minimumLength, maximumTokens);
            return tokens;
        }

        private static void AddAsciiToken(List<string> tokens, StringBuilder current, int minimumLength, int maximumTokens)
        {
            if (tokens == null || current == null || tokens.Count >= maximumTokens)
            {
                if (current != null)
                {
                    current.Length = 0;
                }

                return;
            }

            string token = StringUtil.TrimStorageString(current.ToString());
            current.Length = 0;

            if (token.Length < minimumLength)
            {
                return;
            }

            if (!tokens.Contains(token))
            {
                tokens.Add(token);
            }
        }

        private static bool IsVendorToken(string token)
        {
            return token.IndexOf("broadcom", StringComparison.OrdinalIgnoreCase) >= 0
                || token.IndexOf("avago", StringComparison.OrdinalIgnoreCase) >= 0
                || token.IndexOf("lsi", StringComparison.OrdinalIgnoreCase) >= 0
                || token.IndexOf("megaraid", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool IsControllerInfoToken(string token)
        {
            return token.IndexOf("raid", StringComparison.OrdinalIgnoreCase) >= 0
                || token.IndexOf("sas", StringComparison.OrdinalIgnoreCase) >= 0
                || token.IndexOf("fusion", StringComparison.OrdinalIgnoreCase) >= 0
                || token.IndexOf("megaraid", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool LooksLikeFirmwareToken(string token)
        {
            bool hasDigit = false;
            bool hasSeparator = false;
            for (int i = 0; i < token.Length; ++i)
            {
                char c = token[i];
                if (char.IsDigit(c))
                {
                    hasDigit = true;
                }

                if (c == '.' || c == '-' || c == '_')
                {
                    hasSeparator = true;
                }
            }

            return hasDigit && (hasSeparator || token.Length <= 16);
        }

        private static bool HasUsefulDeviceIdentity(StorageDevice device)
        {
            return !string.IsNullOrWhiteSpace(device.ProductName)
                || !string.IsNullOrWhiteSpace(device.SerialNumber)
                || !string.IsNullOrWhiteSpace(device.ProductRevision);
        }

        private static T ResolveDelegate<T>(IntPtr module, string name, out bool found)
            where T : class
        {
            var export = Kernel32Native.GetProcAddress(module, name);

            found = export != IntPtr.Zero;
            if (!found)
            {
                return null;
            }

            return Marshal.GetDelegateForFunctionPointer<T>(export);
        }

        private bool TryInitializeLibrary(out uint[] controllerIds)
        {
            controllerIds = [];

            if (!ExecuteProcessLibCommand(BuildMegaRaidCommand(0U, 1U, 0U, 0U, 0U, 260U), 260, out var output))
            {
                return false;
            }

            if (output == null || output.Length < 8)
            {
                return false;
            }

            int controllerCount = BitConverter.ToUInt16(output, 0);
            if (controllerCount <= 0)
            {
                controllerIds = [];
                return true;
            }

            var ids = new uint[controllerCount];
            int actual = 0;
            for (int i = 0; i < controllerCount; ++i)
            {
                int offset = 4 + (i * 4);
                if (offset + 4 > output.Length)
                {
                    break;
                }

                uint id = BitConverter.ToUInt32(output, offset);
                if (id == 0)
                {
                    continue;
                }

                ids[actual++] = id;
            }

            if (actual != ids.Length)
            {
                var trimmed = new uint[actual];
                Array.Copy(ids, 0, trimmed, 0, actual);
                ids = trimmed;
            }

            controllerIds = ids;
            return true;
        }

        private bool TryGetControllerInfo(uint controllerId, out byte[] data)
        {
            return ExecuteProcessLibCommand(BuildMegaRaidCommand(1U, 0U, 0U, controllerId, 0U, 2464U), 2464, out data);
        }

        private bool TryGetPhysicalDriveList(uint controllerId, out byte[] physicalDriveIds)
        {
            physicalDriveIds = [];

            if (!ExecuteProcessLibCommand(BuildMegaRaidCommand(1U, 0U, 4U, controllerId, 0U, 2048U), 2048, out var output))
            {
                return false;
            }

            if (output == null || output.Length < 8)
            {
                return false;
            }

            int count = (int)BitConverter.ToUInt32(output, 4);
            if (count <= 0)
            {
                physicalDriveIds = [];
                return true;
            }

            var ids = new byte[count];
            int actual = 0;

            for (int i = 0; i < count; ++i)
            {
                int offset = 8 + (i * 24);
                if (offset >= output.Length)
                {
                    break;
                }

                byte id = output[offset];
                ids[actual++] = id;
            }

            if (actual != ids.Length)
            {
                var trimmed = new byte[actual];
                Array.Copy(ids, 0, trimmed, 0, actual);
                ids = trimmed;
            }

            physicalDriveIds = ids;
            return true;
        }

        private bool TryGetPhysicalDriveInfo(StorageDevice device, uint controllerId, byte physicalDriveId)
        {
            if (!ExecuteProcessLibCommand(BuildMegaRaidCommand(2U, 0U, 0U, controllerId, physicalDriveId, 512U), 512, out var output))
            {
                ProbeTraceRecorder.Add(device, $"Vendor backend: MegaRAID physical drive info failed for controller {controllerId}, physical drive {physicalDriveId}.");
                return false;
            }

            if (output == null || output.Length < 512)
            {
                return false;
            }

            StandardAtaProbe.ApplyAtaIdentify(device, output);
            if (device.TransportKind == StorageTransportKind.Unknown)
            {
                device.TransportKind = StorageTransportKind.Raid;
            }

            if (device.Controller.Family == StorageControllerFamily.Unknown)
            {
                device.Controller.Family = StorageControllerFamily.MegaRaid;
            }

            if (string.IsNullOrWhiteSpace(device.Controller.Kind))
            {
                device.Controller.Kind = StorageTextConstants.MegaRaid;
            }

            ProbeTraceRecorder.Add(device, $"Vendor backend: MegaRAID physical drive info succeeded for controller {controllerId}, physical drive {physicalDriveId}.");
            return HasUsefulDeviceIdentity(device);
        }

        private bool TryPassThroughPhysicalDrive(StorageDevice device, uint controllerId, byte physicalDriveId)
        {
            bool success = false;

            if (!HasUsefulDeviceIdentity(device) && TryIdentifyViaStpPassThrough(controllerId, physicalDriveId, out var identifyData))
            {
                StandardAtaProbe.ApplyAtaIdentify(device, identifyData);
                if (device.TransportKind == StorageTransportKind.Unknown)
                {
                    device.TransportKind = StorageTransportKind.Raid;
                }

                if (device.Controller.Family == StorageControllerFamily.Unknown)
                {
                    device.Controller.Family = StorageControllerFamily.MegaRaid;
                }

                if (string.IsNullOrWhiteSpace(device.Controller.Kind))
                {
                    device.Controller.Kind = StorageTextConstants.MegaRaid;
                }

                ProbeTraceRecorder.Add(device, $"Vendor backend: MegaRAID STP IDENTIFY succeeded for controller {controllerId}, physical drive {physicalDriveId}.");
                success = HasUsefulDeviceIdentity(device);
            }

            if (!device.SupportsSmart)
            {
                byte[] smartData = null;
                byte[] smartThresholds = null;

                bool smartOk =
                    (TryReadSmartViaStpPassThrough(controllerId, physicalDriveId, false, out smartData)
                        && TryReadSmartViaStpPassThrough(controllerId, physicalDriveId, true, out smartThresholds))
                    || (TryEnableSmartViaStpPassThrough(controllerId, physicalDriveId)
                        && TryReadSmartViaStpPassThrough(controllerId, physicalDriveId, false, out smartData)
                        && TryReadSmartViaStpPassThrough(controllerId, physicalDriveId, true, out smartThresholds));

                if (smartOk)
                {
                    var attributes = SmartProbe.ParseSmartPages(smartData, smartThresholds);
                    if (attributes.Count > 0)
                    {
                        device.SupportsSmart = true;
                        device.SmartAttributes = attributes;
                        ProbeTraceRecorder.Add(device, $"Vendor backend: MegaRAID STP SMART succeeded for controller {controllerId}, physical drive {physicalDriveId}.");
                        success = true;
                    }
                }
            }

            return success;
        }

        private bool TryIdentifyViaStpPassThrough(uint controllerId, byte physicalDriveId, out byte[] data)
        {
            data = null;

            var passThroughBuffer = new byte[592];
            passThroughBuffer[0] = physicalDriveId;
            passThroughBuffer[1] = 2;
            WriteUInt16(passThroughBuffer, 2, 60);
            WriteUInt32(passThroughBuffer, 4, 17U);
            passThroughBuffer[8] = 39;
            passThroughBuffer[9] |= 0x80;
            passThroughBuffer[10] = 0xEC;
            passThroughBuffer[28] = 1;
            WriteUInt32(passThroughBuffer, 32, 20U);
            WriteUInt32(passThroughBuffer, 36, 512U);

            if (!ExecuteMegaRaidPassThrough(6, 2, controllerId, passThroughBuffer, out passThroughBuffer))
            {
                return false;
            }

            data = new byte[512];
            Buffer.BlockCopy(passThroughBuffer, 60, data, 0, data.Length);
            return true;
        }

        private bool TryReadSmartViaStpPassThrough(uint controllerId, byte physicalDriveId, bool thresholds, out byte[] data)
        {
            data = null;

            var passThroughBuffer = new byte[592];

            passThroughBuffer[0] = physicalDriveId;
            passThroughBuffer[1] = 2;
            WriteUInt16(passThroughBuffer, 2, 60);
            WriteUInt32(passThroughBuffer, 4, 17U);
            passThroughBuffer[8] = 39;
            passThroughBuffer[9] |= 0x80;
            passThroughBuffer[10] = 0xB0;
            passThroughBuffer[11] = (byte)(thresholds ? 0xD1 : 0xD0);
            passThroughBuffer[12] = 0x00;
            passThroughBuffer[13] = 0x4F;
            passThroughBuffer[14] = 0xC2;
            passThroughBuffer[20] = 1;
            passThroughBuffer[28] = 1;
            WriteUInt32(passThroughBuffer, 32, 20U);
            WriteUInt32(passThroughBuffer, 36, 512U);

            if (!ExecuteMegaRaidPassThrough(6, 2, controllerId, passThroughBuffer, out passThroughBuffer))
            {
                return false;
            }

            data = new byte[512];
            Buffer.BlockCopy(passThroughBuffer, 60, data, 0, data.Length);
            return true;
        }

        private bool TryEnableSmartViaStpPassThrough(uint controllerId, byte physicalDriveId)
        {
            var passThroughBuffer = new byte[592];

            passThroughBuffer[0] = physicalDriveId;
            passThroughBuffer[1] = 2;
            WriteUInt16(passThroughBuffer, 2, 60);
            WriteUInt32(passThroughBuffer, 4, 17U);
            passThroughBuffer[8] = 39;
            passThroughBuffer[9] |= 0x80;
            passThroughBuffer[10] = 0xB0;
            passThroughBuffer[11] = 0xD8;
            passThroughBuffer[12] = 0x00;
            passThroughBuffer[13] = 0x4F;
            passThroughBuffer[14] = 0xC2;
            passThroughBuffer[20] = 1;
            passThroughBuffer[28] = 1;
            WriteUInt32(passThroughBuffer, 32, 20U);
            WriteUInt32(passThroughBuffer, 36, 512U);

            return ExecuteMegaRaidPassThrough(6, 2, controllerId, passThroughBuffer, out var _);
        }

        private bool ExecuteMegaRaidPassThrough(byte commandType, byte subCommand, uint controllerId, byte[] inputBuffer, out byte[] outputBuffer)
        {
            outputBuffer = [];
            if (_processLibCommand == null || inputBuffer == null)
            {
                return false;
            }

            var requestPtr = IntPtr.Zero;
            var bufferPtr = IntPtr.Zero;

            try
            {
                bufferPtr = Marshal.AllocHGlobal(inputBuffer.Length);
                Marshal.Copy(inputBuffer, 0, bufferPtr, inputBuffer.Length);

                var request = new MegaRaidPassThroughCommand();
                request.CommandType = commandType;
                request.SubCommand = subCommand;
                request.ControllerID = controllerId;
                request.BufferLength = (uint)inputBuffer.Length;
                request.Buffer = bufferPtr;

                requestPtr = Marshal.AllocHGlobal(Marshal.SizeOf<MegaRaidPassThroughCommand>());
                Marshal.StructureToPtr(request, requestPtr, false);

                int status = _processLibCommand(requestPtr);
                if (status != 0)
                {
                    return false;
                }

                outputBuffer = new byte[inputBuffer.Length];
                Marshal.Copy(bufferPtr, outputBuffer, 0, outputBuffer.Length);
                return true;
            }
            finally
            {
                if (requestPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(requestPtr);
                }
                if (bufferPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(bufferPtr);
                }
            }
        }

        private void ApplyControllerInfoFromBuffer(StorageDevice device, byte[] data, uint controllerId)
        {
            if (device == null || data == null || data.Length == 0)
            {
                return;
            }

            var tokens = ExtractAsciiTokens(data, 4, 12);
            if (tokens.Count == 0)
            {
                return;
            }

            for (int i = 0; i < tokens.Count; ++i)
            foreach (var token in tokens)
            {
                if (string.IsNullOrWhiteSpace(device.VendorName) && IsVendorToken(token))
                {
                    device.VendorName = token;
                    continue;
                }

                if (string.IsNullOrWhiteSpace(device.Controller.Name) && IsControllerInfoToken(token))
                {
                    device.Controller.Name = token;
                    continue;
                }

                if (string.IsNullOrWhiteSpace(device.Controller.Identifier) && LooksLikeFirmwareToken(token))
                {
                    device.Controller.Identifier = token;
                }
            }

            ProbeTraceRecorder.Add(device, $"Vendor backend: MegaRAID controller info parsed for controller id {controllerId} (tokens={tokens.Count}).");
        }

        private MegaRaidProcessLibCommand BuildMegaRaidCommand(uint commandType, uint initializeFlag, uint subCommand, uint controllerId, uint physicalDriveId, uint outputSize)
        {
            var command = new MegaRaidProcessLibCommand();
            command.CommandType = commandType;
            command.InitializeFlag = initializeFlag;
            command.SubCommand = subCommand;
            command.ControllerID = controllerId;
            command.PhysicalDriveID = physicalDriveId;
            command.OutputSize = outputSize;
            command.OutputBuffer = IntPtr.Zero;
            return command;
        }

        private bool ExecuteProcessLibCommand(MegaRaidProcessLibCommand command, int outputSize, out byte[] output)
        {
            output = null;
            if (_processLibCommand == null || outputSize <= 0)
            {
                return false;
            }

            var buffer = new byte[outputSize];
            var outputHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            var requestPtr = IntPtr.Zero;

            try
            {
                command.OutputBuffer = outputHandle.AddrOfPinnedObject();

                requestPtr = Marshal.AllocHGlobal(Marshal.SizeOf<MegaRaidProcessLibCommand>());
                Marshal.StructureToPtr(command, requestPtr, false);

                int result = _processLibCommand(requestPtr);
                if (result != 0)
                {
                    return false;
                }
            }
            finally
            {
                if (requestPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(requestPtr);
                }

                outputHandle.Free();
            }

            output = buffer;
            return true;
        }

        private void EnsureExportsResolved()
        {
            if (_exportsResolved)
            {
                return;
            }

            _exportsResolved = true;

            var handle = _libraries.GetMegaRaidLibrary();
            if (handle == null || handle.IsInvalid)
            {
                return;
            }

            var module = handle.DangerousGetHandle();

            _processLibCommand = ResolveDelegate<ProcessLibCommandDelegate>(module, "ProcessLibCommand", out var found);
            _capabilities.HasProcessLibCommand = found;
        }

        #endregion
    }
}
