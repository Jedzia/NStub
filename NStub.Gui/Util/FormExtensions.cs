// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormExtensions.cs" company="EvePanix">
//   (c) 2009 Jedzia
// </copyright>
// <summary>
//   Extension Methods for the <see cref="Form" /> class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NStub.Gui.Util
{
    #region Imports

    using System;
    using System.Windows.Forms;

    #endregion

    /// <summary>
    /// Extension Methods for the <see cref="Form"/> class.
    /// </summary>
    public static class FormExtensions
    {
        /// <summary>
        /// Automates the attach and detachment of the load event to an <see cref="Form"/>.
        /// </summary>
        /// <param name="value">The control to be added.</param>
        /// <param name="formBackField">The backing field for the Form.</param>
        /// <param name="formBackFieldEvent">The backing fields event to attach to.</param>
        /// <param name="eventHandler">The event handler that is attached to the formBackFieldEvent.</param>
        /// <returns>
        /// false if value was null or the same like in the backing field.
        /// </returns>
        public static bool AttachToEvent(
            this object value, 
            ref object formBackField, 
            ref EventHandler formBackFieldEvent, 
            EventHandler eventHandler)
        {
            if (value == null || value == formBackField)
            {
                return false;
            }

            if (formBackField != null)
            {
                formBackFieldEvent -= eventHandler;
            }

            formBackField = value;
            formBackFieldEvent += eventHandler;
            return true;
        }

        /// <summary>
        /// Automates the attach and detachment of the load event to an <see cref="Form"/>.
        /// </summary>
        /// <param name="value">The control to be added.</param>
        /// <param name="formBackField">The backing field for the Form.</param>
        /// <param name="onLoadEventHandler">The load event handler.</param>
        /// <returns>false if value was null or the same like in the backing field.</returns>
        public static bool AttachToLoadEvent(this Form value, ref Form formBackField, EventHandler onLoadEventHandler)
        {
            if (value == null || value == formBackField)
            {
                return false;
            }

            if (formBackField != null)
            {
                formBackField.Load -= onLoadEventHandler;
            }

            formBackField = value;
            formBackField.Load += onLoadEventHandler;
            return true;
        }

        /// <summary>
        /// Automates the attach and detachment of the load event to an <see cref="UserControl"/>.
        /// </summary>
        /// <param name="value">The control to be added.</param>
        /// <param name="userControlBackField">The backing field for the UserControl.</param>
        /// <param name="onLoadEventHandler">The load event handler.</param>
        /// <returns>false if value was null or the same like in the backing field.</returns>
        public static bool AttachToLoadEvent(this UserControl value, ref UserControl userControlBackField, EventHandler onLoadEventHandler)
        {
            if (value == null || value == userControlBackField)
            {
                return false;
            }

            if (userControlBackField != null)
            {
                userControlBackField.Load -= onLoadEventHandler;
            }

            userControlBackField = value;
            userControlBackField.Load += onLoadEventHandler;
            return true;
        }
    }
}