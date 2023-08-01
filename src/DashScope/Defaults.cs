using System;
using System.Collections.Generic;
using System.Text;

namespace DashScope
{
    public class Defaults
    {

        public static string ApiBase = "https://dashscope.aliyuncs.com/api";

        /// <summary>
        /// 获取API
        /// </summary>
        /// <param name="apiVersion">API 版本，固定取值v1</param>
        /// <param name="taskGroup">任务组:<br/>生成aigc<br/>嵌入embeddings</param>
        /// <param name="task">推理任务名称:<br/>文本生成text-generation<br/>嵌入text-embedding<br/>文生图text2image</param>
        /// <param name="functionCall">调用模型特定功能<br/>文本生成generation<br/>嵌入text-embedding<br/>文生图image-synthesis</param>
        /// <returns></returns>
        public static string GetApiEndpoint(string apiVersion = "v1", string taskGroup = "aigc", string task = "text-generation", string functionCall = "generation")
        {
            return $"{ApiBase}/{apiVersion}/services/{taskGroup}/{task}/{functionCall}";
        }
    }
}
