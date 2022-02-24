// Copyright (c) usercode
// https://github.com/usercode/ImageWizard
// MIT License

using ImageWizard.Processing;
using ImageWizard.Processing.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImageWizard
{
    /// <summary>
    /// FilterContext
    /// </summary>
    public abstract class FilterContext : IDisposable
    {
        public FilterContext(PipelineContext context)
        {
            ProcessingContext = context;
        }

        /// <summary>
        /// ProcessingContext
        /// </summary>
        public PipelineContext ProcessingContext { get; }

        private DataResult? _result;

        /// <summary>
        /// Result
        /// </summary>
        public DataResult? Result 
        {
            get => _result;
            set
            {
                _result?.Dispose();

                _result = value;
            }
        }

        public virtual void Dispose()
        {
            //Result?.Dispose();
        }

        public abstract Task<DataResult> BuildResultAsync();
    }
}
