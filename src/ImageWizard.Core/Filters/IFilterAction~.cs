﻿// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using System;
using System.Collections.Generic;
using System.Text;

namespace ImageWizard
{
    /// <summary>
    /// IFilterAction
    /// </summary>
    public interface IFilterAction<TFilerContext> : IFilterAction
        where TFilerContext : FilterContext
    {
        /// <summary>
        /// TryExecute
        /// </summary>
        /// <param name="input"></param>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        bool TryExecute(string input, TFilerContext filterContext);
    }
}
