/*
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 *
 * Copyright (c) 2026 Florian K.
 */

using CommunityToolkit.Mvvm.ComponentModel;
using DiskInfoViewer.ModelAbstraction;

namespace DiskInfoViewer.ViewModels
{
    public partial class StorageViewModel : ViewModelBase
    {
        #region Constructor

        public StorageViewModel(StorageVM storage)
        {
            storage.Update();
            Storage = storage;

            _UpdateTask = new Task(UpdateStorage);
            _UpdateTask.Start();
        }

        #endregion

        #region Fields

        Task _UpdateTask;

        #endregion

        #region Properties

        [ObservableProperty]
        StorageVM _storage;

        #endregion

        #region Private

        void UpdateStorage()
        {
            const int UpdateTimeInMilliseconds = 2500;

            while (true)
            {
                Thread.Sleep(UpdateTimeInMilliseconds);

                Storage.Update();
            }
        }

        #endregion
    }
}
