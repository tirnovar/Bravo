﻿using Sqlbi.Bravo.Infrastructure.Extensions;
using Sqlbi.Bravo.Infrastructure.Windows;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace Sqlbi.Bravo.Infrastructure.Helpers
{
    internal static class WindowDialogHelper
    {
        private static void RunDialog(Action action)
        {
            var threadStart = new ThreadStart(action);
            var thread = new Thread(threadStart);
            thread.CurrentCulture = thread.CurrentUICulture = CultureInfo.CurrentCulture;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        public static (bool canceled, string? path) OpenFileDialog(string defaultExt, CancellationToken cancellationToken)
        {
            var dialogOwner = Win32WindowWrapper.CreateFrom(Process.GetCurrentProcess().MainWindowHandle);
            var dialogResult = System.Windows.Forms.DialogResult.None;
            var defaultExtLowercase = defaultExt.ToLower();

            using var dialog = new System.Windows.Forms.OpenFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments, Environment.SpecialFolderOption.DoNotVerify),
                Filter = $"{ defaultExt } files (*.{ defaultExtLowercase })|*.{ defaultExtLowercase }|All files (*.*)|*.*",
                Title = "Open file",
                ShowReadOnly = false,
                CheckFileExists = true,
                DefaultExt = defaultExtLowercase
            };

            if (!cancellationToken.IsCancellationRequested)
            {
                RunDialog(() => dialogResult = dialog.ShowDialog(dialogOwner));

                //var dialog2 = new Bravo.Infrastructure.Windows.SaveFileDialog
                //{
                //    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments, Environment.SpecialFolderOption.DoNotVerify),
                //    Filter = "Vpax files (*.vpax)|*.vpax|All files (*.*)|*.*",
                //    Title = "Export file",
                //    DefaultExt = "vpax",
                //    //FileName = fileName
                //};
                //var result = dialog2.ShowDialog(hWnd: Process.GetCurrentProcess().MainWindowHandle);
            }

            var canceled = dialogResult == System.Windows.Forms.DialogResult.Cancel;
            var path = dialog.FileName.NullIfEmpty();

            return (canceled, path);
        }

        public static (bool canceled, string? path) SaveFileDialog(string? fileName, string defaultExt, CancellationToken cancellationToken)
        {
            var dialogOwner = Win32WindowWrapper.CreateFrom(Process.GetCurrentProcess().MainWindowHandle);
            var dialogResult = System.Windows.Forms.DialogResult.None;
            var defaultExtLowercase = defaultExt.ToLower();

            using var dialog = new System.Windows.Forms.SaveFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments, Environment.SpecialFolderOption.DoNotVerify),
                Filter = $"{ defaultExt } files (*.{ defaultExtLowercase })|*.{ defaultExtLowercase }|All files (*.*)|*.*",
                Title = "Save file",
                DefaultExt = defaultExtLowercase,
                FileName = fileName
            };

            if (!cancellationToken.IsCancellationRequested)
            {
                RunDialog(() => dialogResult = dialog.ShowDialog(dialogOwner));

                //var dialog2 = new Bravo.Infrastructure.Windows.SaveFileDialog
                //{
                //    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments, Environment.SpecialFolderOption.DoNotVerify),
                //    Filter = "Vpax files (*.vpax)|*.vpax|All files (*.*)|*.*",
                //    Title = "Export file",
                //    DefaultExt = "vpax",
                //    //FileName = fileName
                //};
                //var result = dialog2.ShowDialog(hWnd: Process.GetCurrentProcess().MainWindowHandle);
            }

            var canceled = dialogResult == System.Windows.Forms.DialogResult.Cancel;
            var path = dialog.FileName.NullIfEmpty();

            return (canceled, path);
        }

        public static (bool canceled, string? path) BrowseFolderDialog(CancellationToken cancellationToken)
        {
            var dialogOwner = Win32WindowWrapper.CreateFrom(Process.GetCurrentProcess().MainWindowHandle);
            var dialogResult = System.Windows.Forms.DialogResult.None;

            using var dialog = new System.Windows.Forms.FolderBrowserDialog()
            {
                RootFolder = Environment.SpecialFolder.MyDocuments,
                ShowNewFolderButton = true,
            };
            
            if (!cancellationToken.IsCancellationRequested)
            {
                RunDialog(() => dialogResult = dialog.ShowDialog(dialogOwner));
            }

            var canceled = dialogResult == System.Windows.Forms.DialogResult.Cancel;
            var path = dialog.SelectedPath.NullIfEmpty();

            return (canceled, path);
        }
    }
}
