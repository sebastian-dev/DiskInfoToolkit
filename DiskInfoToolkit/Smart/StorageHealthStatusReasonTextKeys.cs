/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

namespace DiskInfoToolkit.Smart
{
    internal static class StorageHealthStatusReasonTextKeys
    {
        #region Fields

        public const string Prefix = "HealthStatus.Reason.";

        public const string AttributeBelowThreshold = Prefix + "AttributeBelowThreshold";

        public const string AttributeRawValueNonZero = Prefix + "AttributeRawValueNonZero";

        public const string NvmeAvailableSpareAtThreshold = Prefix + "NvmeAvailableSpareAtThreshold";

        public const string NvmeAvailableSpareBelowThreshold = Prefix + "NvmeAvailableSpareBelowThreshold";

        public const string NvmePersistentMemoryReadOnly = Prefix + "NvmePersistentMemoryReadOnly";

        public const string NvmeReadOnlyMode = Prefix + "NvmeReadOnlyMode";

        public const string NvmeSubsystemReliabilityDegraded = Prefix + "NvmeSubsystemReliabilityDegraded";

        public const string NvmeTemperatureError = Prefix + "NvmeTemperatureError";

        public const string NvmeVolatileMemoryBackupFailed = Prefix + "NvmeVolatileMemoryBackupFailed";

        public const string RemainingLifeCritical = Prefix + "RemainingLifeCritical";

        public const string RemainingLifeLow = Prefix + "RemainingLifeLow";

        public const string RemainingLifeVeryLow = Prefix + "RemainingLifeVeryLow";

        #endregion
    }
}
