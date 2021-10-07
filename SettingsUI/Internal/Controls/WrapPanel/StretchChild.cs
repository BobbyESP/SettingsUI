﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace SettingsUI.Internal.Controls
{
    /// <summary>
    /// Options for how to calculate the layout of <see cref="Microsoft.UI.Xaml.Controls.WrapGrid"/> items.
    /// </summary>
    internal enum StretchChild
    {
        /// <summary>
        /// Don't apply any additional stretching logic
        /// </summary>
        None,

        /// <summary>
        /// Make the last child stretch to fill the available space
        /// </summary>
        Last
    }
}
