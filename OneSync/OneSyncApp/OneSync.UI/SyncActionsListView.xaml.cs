﻿//Coded by Koh Eng Tat Desmond
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using OneSync.Synchronization;
using System.Resources;

namespace OneSync.UI
{
	public partial class SyncActionsListView
	{
        public ResourceManager m_ResourceManager = new ResourceManager(Properties.Settings.Default.LanguageResx,
                                    System.Reflection.Assembly.GetExecutingAssembly());

		public SyncActionsListView()
		{
			this.InitializeComponent();

            language();
		}

        private void language()
        {
            SyncActionListView_header_1.Text = m_ResourceManager.GetString("hdr_skip");
            SyncActionListView_header_2.Header = m_ResourceManager.GetString("hdr_action");
            SyncActionListView_header_3.Header = m_ResourceManager.GetString("hdr_filePath");
            SyncActionListView_header_4.Header = m_ResourceManager.GetString("hdr_originalFilePath");
            SyncActionListView_header_5.Header = m_ResourceManager.GetString("hdr_conflictResolution");
        }

        public IEnumerable<SyncAction> ItemsSource
        {
            get { return (IEnumerable<SyncAction>)lvActions.ItemsSource; }
            set { lvActions.ItemsSource = value; }
        }

        private void checkBoxSelectAll_Checked(object sender, RoutedEventArgs e)
        {
            UpdateSkipStatus(true);
        }

        private void checkBoxSelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateSkipStatus(false);
        }

        private void UpdateSkipStatus(bool skipAll)
        {
            IEnumerable<SyncAction> actions = (IEnumerable<SyncAction>)lvActions.ItemsSource;
            if (actions == null) return;

            foreach (SyncAction action in actions)
                action.Skip = skipAll;
        }
    }

    #region Converters
	
	[ValueConversion(typeof(bool), typeof(bool))]
    public class InvertBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
    
    [ValueConversion(typeof(ConflictResolution), typeof(Visibility))]
    public class ConflictVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((ConflictResolution)value != ConflictResolution.NONE)
                return Visibility.Visible;
            else
                return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }


    [ValueConversion(typeof(ConflictResolution), typeof(int))]
    public class ConflictResolutionToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ConflictResolution)value)
            {
                case ConflictResolution.DUPLICATE_RENAME:
                    return 0;
                case ConflictResolution.OVERWRITE:
                    return 1;
                case ConflictResolution.NONE:
                    return -1;
                default:
                    return -1;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 0:
                    return ConflictResolution.DUPLICATE_RENAME;
                case 1:
                    return ConflictResolution.OVERWRITE;
                case -1:
                    return ConflictResolution.NONE;
                default:
                    return ConflictResolution.NONE;
            }
        }
    }


    [ValueConversion(typeof(ChangeType), typeof(string))]
    public class ChangeTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ChangeType)value)
            {
                case ChangeType.NEWLY_CREATED:
                    return "Copy";
                case ChangeType.DELETED:
                    return "Delete";
                case ChangeType.MODIFIED:
                    return "Copy";
                case ChangeType.RENAMED:
                    return "Rename";
                default:
                    return "Unknown";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    [ValueConversion(typeof(ChangeType), typeof(string))]
    public class ChangeTypeColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ChangeType)value)
            {
                case ChangeType.NEWLY_CREATED:
                    return "Green";
                case ChangeType.DELETED:
                    return "Red";
                case ChangeType.MODIFIED:
                    return "Blue";
                case ChangeType.RENAMED:
                    return "Black";
                default:
                    return "Black";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    [ValueConversion(typeof(SyncAction), typeof(string))]
    public class OriginalPathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SyncAction action = (SyncAction)value;
            if (action.ChangeType == ChangeType.RENAMED)
                return ((RenameAction)action).PreviousRelativeFilePath;
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }


    #endregion

}