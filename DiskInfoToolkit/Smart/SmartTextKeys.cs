/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Smart
{
    public static class SmartTextKeys
    {
        #region Fields

        public const string AttributePrefix = "Smart.Attribute.";

        public const string AdaptiveReadRetryAttempts = "AdaptiveReadRetryAttempts";

        public const string AirflowTemperature = "AirflowTemperature";

        public const string AvailableReservedSpace = "AvailableReservedSpace";

        public const string AvailableSpare = "AvailableSpare";

        public const string AvailableSpareThreshold = "AvailableSpareThreshold";

        public const string AverageEraseCount = "AverageEraseCount";

        public const string AverageEraseCountMaxEraseCount = "AverageEraseCountMaxEraseCount";

        public const string AverageNANDEraseCount = "AverageNANDEraseCount";

        public const string AveragePECycles = "AveragePECycles";

        public const string BackgroundProgramPageCount = "BackgroundProgramPageCount";

        public const string BadBlockCount = "BadBlockCount";

        public const string BadBlockFullFlag = "BadBlockFullFlag";

        public const string BadClusterTableCount = "BadClusterTableCount";

        public const string BitErrorCount = "BitErrorCount";

        public const string CRCErrorCount = "CRCErrorCount";

        public const string CapacitorHealth = "CapacitorHealth";

        public const string CleanShutdownCount = "CleanShutdownCount";

        public const string CommandTimeout = "CommandTimeout";

        public const string CompositeTemperature = "CompositeTemperature";

        public const string ControllerBusyTime = "ControllerBusyTime";

        public const string CriticalCompositeTemperatureTime = "CriticalCompositeTemperatureTime";

        public const string CriticalWarning = "CriticalWarning";

        public const string CumulativeProgramNANDPages = "CumulativeProgramNANDPages";

        public const string CurrentHeliumLevel = "CurrentHeliumLevel";

        public const string DASPolarity = "DASPolarity";

        public const string DRAM1BitErrorCount = "DRAM1BitErrorCount";

        public const string DataAddressMarkError = "DataAddressMarkError";

        public const string DeviceCapacity = "DeviceCapacity";

        public const string DiskShift = "DiskShift";

        public const string DriveLifeProtectionStatus = "DriveLifeProtectionStatus";

        public const string ECCBitCorrectionCount = "ECCBitCorrectionCount";

        public const string ECCBitsCorrected = "ECCBitsCorrected";

        public const string ECCCumulativeThresholdEvents = "ECCCumulativeThresholdEvents";

        public const string ECCErrorRate = "ECCErrorRate";

        public const string ECCFailRecord = "ECCFailRecord";

        public const string EndToEndError = "EndToEndError";

        public const string EndToEndErrorsCorrected = "EndToEndErrorsCorrected";

        public const string EraseFailCount = "EraseFailCount";

        public const string EraseFailCountInWorstDie = "EraseFailCountInWorstDie";

        public const string EraseFailCountWorstCase = "EraseFailCountWorstCase";

        public const string EraseFailureBlockCount = "EraseFailureBlockCount";

        public const string ErrorCorrectionCount = "ErrorCorrectionCount";

        public const string ErrorDetection = "ErrorDetection";

        public const string FirmwareVersion = "FirmwareVersion";

        public const string FlashReadSectorCount = "FlashReadSectorCount";

        public const string FlashWriteSectorCount = "FlashWriteSectorCount";

        public const string FlyingHeight = "FlyingHeight";

        public const string FreeFallProtection = "FreeFallProtection";

        public const string FreeSpace = "FreeSpace";

        public const string GDN = "GDN";

        public const string GMRHeadAmplitude = "GMRHeadAmplitude";

        public const string GSenseErrorRate = "GSenseErrorRate";

        public const string GigabytesErased = "GigabytesErased";

        public const string GoodBlockCountSystemBlockCount = "GoodBlockCountSystemBlockCount";

        public const string GrownBadBlocks = "GrownBadBlocks";

        public const string HaltSystemIDFlashID = "HaltSystemIDFlashID";

        public const string HardwareECCRecovered = "HardwareECCRecovered";

        public const string HeadFlyingHours = "HeadFlyingHours";

        public const string HeliumConditionLower = "HeliumConditionLower";

        public const string HeliumConditionUpper = "HeliumConditionUpper";

        public const string HighFlyWrites = "HighFlyWrites";

        public const string HostDataRead = "HostDataRead";

        public const string HostDataWritten = "HostDataWritten";

        public const string HostProgramPageCount = "HostProgramPageCount";

        public const string HostReadCommands = "HostReadCommands";

        public const string HostWriteCommands = "HostWriteCommands";

        public const string IOErrorDetectionCodeErrors = "IOErrorDetectionCodeErrors";

        public const string InWarranty = "InWarranty";

        public const string LifetimePS3EntryCount = "LifetimePS3EntryCount";

        public const string LifetimePS4EntryCount = "LifetimePS4EntryCount";

        public const string LifetimeUsed = "LifetimeUsed";

        public const string LoadFriction = "LoadFriction";

        public const string LoadInTime = "LoadInTime";

        public const string LoadUnloadCycleCount = "LoadUnloadCycleCount";

        public const string LoadedHours = "LoadedHours";

        public const string MAMRHealthMonitor = "MAMRHealthMonitor";

        public const string MaxEraseCountOfSpec = "MaxEraseCountOfSpec";

        public const string MaxRatedPECounts = "MaxRatedPECounts";

        public const string MaximumBadBlocksPerDie = "MaximumBadBlocksPerDie";

        public const string MaximumEraseCount = "MaximumEraseCount";

        public const string MaximumNANDEraseCount = "MaximumNANDEraseCount";

        public const string MaximumPECountSpecification = "MaximumPECountSpecification";

        public const string MaximumPECycles = "MaximumPECycles";

        public const string MediaAndDataIntegrityErrors = "MediaAndDataIntegrityErrors";

        public const string MediaWearoutIndicator = "MediaWearoutIndicator";

        public const string MinimumEraseCount = "MinimumEraseCount";

        public const string MinimumNANDEraseCount = "MinimumNANDEraseCount";

        public const string MinimumPECycles = "MinimumPECycles";

        public const string NANDTemperature = "NANDTemperature";

        public const string NandDataRead = "NandDataRead";

        public const string NandDataWritten = "NandDataWritten";

        public const string NandReadRetryCount = "NandReadRetryCount";

        public const string Non4KAlignedAccess = "Non4KAlignedAccess";

        public const string NumberOfCacheDataBlock = "NumberOfCacheDataBlock";

        public const string NumberOfErrorInformationLogEntries = "NumberOfErrorInformationLogEntries";

        public const string NumberOfInvalidBlocks = "NumberOfInvalidBlocks";

        public const string OfflineSeekPerformance = "OfflineSeekPerformance";

        public const string OfflineUncorrectableErrors = "OfflineUncorrectableErrors";

        public const string PECycles = "PECycles";

        public const string PORRecoveryCount = "PORRecoveryCount";

        public const string PartialPfail = "PartialPfail";

        public const string PendingSectorCount = "PendingSectorCount";

        public const string PercentOfTotalEraseCount = "PercentOfTotalEraseCount";

        public const string PercentOfTotalEraseCountBCBlocks = "PercentOfTotalEraseCountBCBlocks";

        public const string PercentageUsed = "PercentageUsed";

        public const string PowerCycleCount = "PowerCycleCount";

        public const string PowerFailBackupHealth = "PowerFailBackupHealth";

        public const string PowerLossProtectionFailure = "PowerLossProtectionFailure";

        public const string PowerOffRetractCount = "PowerOffRetractCount";

        public const string PowerOnHours = "PowerOnHours";

        public const string ProgramFailCount = "ProgramFailCount";

        public const string ProgramFailCountInWorstDie = "ProgramFailCountInWorstDie";

        public const string ProgramFailCountWorstCase = "ProgramFailCountWorstCase";

        public const string ProgramFailureBlockCount = "ProgramFailureBlockCount";

        public const string RAIDEventCount = "RAIDEventCount";

        public const string RAIDRecoveryCount = "RAIDRecoveryCount";

        public const string RAIDUncorrectableCount = "RAIDUncorrectableCount";

        public const string RAISEECCCorrectableCount = "RAISEECCCorrectableCount";

        public const string RawDataErrorRate = "RawDataErrorRate";

        public const string ReadChannelMargin = "ReadChannelMargin";

        public const string ReadErrorRate = "ReadErrorRate";

        public const string ReadErrorRetryRate = "ReadErrorRetryRate";

        public const string ReadFailureBlockCount = "ReadFailureBlockCount";

        public const string ReallocatedNANDBlocks = "ReallocatedNANDBlocks";

        public const string ReallocatedSectorsCount = "ReallocatedSectorsCount";

        public const string ReallocationEventCount = "ReallocationEventCount";

        public const string RecalibrationRetries = "RecalibrationRetries";

        public const string RemainingLife = "RemainingLife";

        public const string RemainingSpareBlocks = "RemainingSpareBlocks";

        public const string ReserveBlockCount = "ReserveBlockCount";

        public const string Reserved = "Reserved";

        public const string RetiredBlockCount = "RetiredBlockCount";

        public const string RunOutCancel = "RunOutCancel";

        public const string RuntimeBadBlock = "RuntimeBadBlock";

        public const string SATADownshiftCount = "SATADownshiftCount";

        public const string SATAPhysicalErrorCount = "SATAPhysicalErrorCount";

        public const string SATARErrors = "SATARErrors";

        public const string SLCAverageEraseCount = "SLCAverageEraseCount";

        public const string SLCCache = "SLCCache";

        public const string SLCMaximumEraseCount = "SLCMaximumEraseCount";

        public const string SLCMinimumEraseCount = "SLCMinimumEraseCount";

        public const string SLCTotalEraseCount = "SLCTotalEraseCount";

        public const string SPITestsRemaining = "SPITestsRemaining";

        public const string SSDModeStatus = "SSDModeStatus";

        public const string SSDProtectMode = "SSDProtectMode";

        public const string SeekErrorRate = "SeekErrorRate";

        public const string SeekTimePerformance = "SeekTimePerformance";

        public const string ShockDuringWrite = "ShockDuringWrite";

        public const string ShockEventCount = "ShockEventCount";

        public const string SimpleReadRetryAttempts = "SimpleReadRetryAttempts";

        public const string SoftECCCorrection = "SoftECCCorrection";

        public const string SoftReadErrorRate = "SoftReadErrorRate";

        public const string SoftReadErrorRateStab = "SoftReadErrorRateStab";

        public const string SpareBlocksAvailable = "SpareBlocksAvailable";

        public const string SpinBuzz = "SpinBuzz";

        public const string SpinHighCurrent = "SpinHighCurrent";

        public const string SpinRetryCount = "SpinRetryCount";

        public const string SpinUpTime = "SpinUpTime";

        public const string StartStopCount = "StartStopCount";

        public const string SuccessfulRAINRecoveryCount = "SuccessfulRAINRecoveryCount";

        public const string SuperCapStatus = "SuperCapStatus";

        public const string Temperature = "Temperature";

        public const string TemperatureSensor1 = "TemperatureSensor1";

        public const string TemperatureSensor2 = "TemperatureSensor2";

        public const string TemperatureSensor3 = "TemperatureSensor3";

        public const string TemperatureSensor4 = "TemperatureSensor4";

        public const string TemperatureSensor5 = "TemperatureSensor5";

        public const string TemperatureSensor6 = "TemperatureSensor6";

        public const string TemperatureSensor7 = "TemperatureSensor7";

        public const string TemperatureSensor8 = "TemperatureSensor8";

        public const string ThermalAsperityRate = "ThermalAsperityRate";

        public const string ThermalManagementTemperature1TransitionCount = "ThermalManagementTemperature1TransitionCount";

        public const string ThermalManagementTemperature2TransitionCount = "ThermalManagementTemperature2TransitionCount";

        public const string ThermalThrottleStatus = "ThermalThrottleStatus";

        public const string ThrottleStatistics = "ThrottleStatistics";

        public const string ThroughputPerformance = "ThroughputPerformance";

        public const string TimedWorkloadHostReadWriteRatio = "TimedWorkloadHostReadWriteRatio";

        public const string TimedWorkloadMediaWear = "TimedWorkloadMediaWear";

        public const string TimedWorkloadTimer = "TimedWorkloadTimer";

        public const string TorqueAmplificationCount = "TorqueAmplificationCount";

        public const string TotalBackgroundScan = "TotalBackgroundScan";

        public const string TotalBackgroundScanOverLimitCount = "TotalBackgroundScanOverLimitCount";

        public const string TotalBlockEraseFailure = "TotalBlockEraseFailure";

        public const string TotalBlockReMapPassCount = "TotalBlockReMapPassCount";

        public const string TotalBlocksErased = "TotalBlocksErased";

        public const string TotalBytesRead = "TotalBytesRead";

        public const string TotalCountErrorBitsFromFlash = "TotalCountErrorBitsFromFlash";

        public const string TotalCountReadCommands = "TotalCountReadCommands";

        public const string TotalCountReadSectors = "TotalCountReadSectors";

        public const string TotalCountReadSectorsWithCorrectableBitErrors = "TotalCountReadSectorsWithCorrectableBitErrors";

        public const string TotalCountWriteCommands = "TotalCountWriteCommands";

        public const string TotalCountWriteSectors = "TotalCountWriteSectors";

        public const string TotalDoRefCalCount = "TotalDoRefCalCount";

        public const string TotalEraseCount = "TotalEraseCount";

        public const string TotalNANDEraseCount = "TotalNANDEraseCount";

        public const string TotalNANDReadPlaneCountHigh = "TotalNANDReadPlaneCountHigh";

        public const string TotalNANDReadPlaneCountLow = "TotalNANDReadPlaneCountLow";

        public const string TotalNumberCorrectedBits = "TotalNumberCorrectedBits";

        public const string TotalReadFailures = "TotalReadFailures";

        public const string TotalRefreshISPCount = "TotalRefreshISPCount";

        public const string TotalTimeThermalManagementTemperature1 = "TotalTimeThermalManagementTemperature1";

        public const string TotalTimeThermalManagementTemperature2 = "TotalTimeThermalManagementTemperature2";

        public const string TotalUncorrectableNANDReads = "TotalUncorrectableNANDReads";

        public const string UnalignedAccessCount = "UnalignedAccessCount";

        public const string UncorrectableErrors = "UncorrectableErrors";

        public const string UncorrectableSectorCount = "UncorrectableSectorCount";

        public const string UncorrectableSoftReadErrorRate = "UncorrectableSoftReadErrorRate";

        public const string UnexpectedPowerLoss = "UnexpectedPowerLoss";

        public const string UnsafeShutdownCount = "UnsafeShutdownCount";

        public const string UnusedReservedBlockCount = "UnusedReservedBlockCount";

        public const string UnusedSpareNANDBlocks = "UnusedSpareNANDBlocks";

        public const string UsedReservedBlockCount = "UsedReservedBlockCount";

        public const string UsedReservedBlockCountWorstCase = "UsedReservedBlockCountWorstCase";

        public const string UsedReservedBlockCountWorstDie = "UsedReservedBlockCountWorstDie";

        public const string UserCapacity = "UserCapacity";

        public const string VendorUnique = "VendorUnique";

        public const string VibrationDuringWrite = "VibrationDuringWrite";

        public const string WarningCompositeTemperatureTime = "WarningCompositeTemperatureTime";

        public const string WearLevelingCount = "WearLevelingCount";

        public const string WearRangeDelta = "WearRangeDelta";

        public const string WriteAmplificationFactor = "WriteAmplificationFactor";

        public const string WriteAmplificationMultipliedBy100 = "WriteAmplificationMultipliedBy100";

        public const string WriteErrorRate = "WriteErrorRate";

        public const string WriteHead = "WriteHead";

        public const string WriteProtectProgress = "WriteProtectProgress";

        public const string WriteThrottlingActivationFlag = "WriteThrottlingActivationFlag";

        #endregion

        #region Public

        public static string GetAttributeNameKey(string attributeKey)
        {
            return string.IsNullOrWhiteSpace(attributeKey)
                ? string.Empty
                : AttributePrefix + attributeKey;
        }

        #endregion
    }
}
