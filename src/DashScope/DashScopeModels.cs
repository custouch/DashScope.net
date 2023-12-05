using System;
using System.Collections.Generic;
using System.Text;

namespace DashScope
{
    public class DashScopeModels
    {
        /// <summary>
        /// 通义千问超大规模语言模型，支持中文英文等不同语言输入。
        /// 模型支持 8k tokens上下文，为了保障正常使用和正常输出，API限定用户输入为6k Tokens。
        /// </summary>
        public const string QWenTurbo = "qwen-turbo";

        /// <summary>
        /// 通义千问超大规模语言模型增强版，支持中文英文等不同语言输入。
        /// 模型支持 8k tokens上下文，为了保障正常使用和正常输出，API限定用户输入为6k Tokens。
        /// </summary>
        public const string QWenPlus = "qwen-plus";


        /// <summary>
        /// 通义千问2.0千亿级别超大规模语言模型，支持中文英文等不同语言输入。
        /// 模型支持 8k tokens上下文，为了保障正常使用和正常输出，API限定用户输入为6k Tokens。
        /// </summary>
        public const string QWenMax = "qwen-max";

        /// <summary>
        /// 通义千问千亿级别超大规模语言模型，支持中文英文等不同语言输入。
        /// 模型支持 30k tokens上下文，为了保障正常的使用和输出，API限定用户输入为 28k tokens。
        /// </summary>
        public const string QWenLongContext = "qwen-max-longcontext";

        /// <summary>
        /// 通用文本向量,支持中文、英语、西班牙语、法语、葡萄牙语、印尼语。
        /// </summary>
        public const string TextEmbeddingV1 = "text-embedding-v1";

        /// <summary>
        /// 通用文本向量,支持中文、英语、西班牙语、法语、葡萄牙语、印尼语。
        /// </summary>
        public const string TextEmbeddingV2 = "text-embedding-v2";

        /// <summary>
        /// 通用文本向量批处理,支持中文、英语、西班牙语、法语、葡萄牙语、印尼语。
        /// </summary>
        public const string TextEmbeddingAsyncV1 = "text-embedding-async-v1";

        // <summary>
        /// 通用文本向量批处理,支持中文、英语、西班牙语、法语、葡萄牙语、印尼语。
        /// </summary>
        public const string TextEmbeddingAsyncV2 = "text-embedding-async-v2";
    }
}
