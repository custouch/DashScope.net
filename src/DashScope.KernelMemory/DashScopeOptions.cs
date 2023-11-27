using DashScope.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DashScope.KernelMemory
{
    /// <summary>
    /// Options for DashScope KernelMemory
    /// </summary>
    public class DashScopeOptions
    {
        /// <summary>
        /// The DashScope model to use, default is <see cref="DashScopeModels.QWenTurbo"/>
        /// </summary>
        public string Model { get; set; } = DashScopeModels.QWenTurbo;


        /// <summary>
        /// The DashScope model to use for text embedding, default is <see cref="DashScopeModels.TextEmbeddingV1"/>
        /// </summary>
        public string EmbeddingModel { get; set; } = DashScopeModels.TextEmbeddingV1;


        /// <summary>
        /// The default completion parameters to use, default is null. 
        /// It will be merged with the <see cref="TextGenerationOptions"/> when generating text.
        /// </summary>
        public CompletionParameters? DefaultCompletionParameters { get; set; }
    }
}
