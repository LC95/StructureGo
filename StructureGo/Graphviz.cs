using System;
using System.IO;
using System.Runtime.InteropServices;

namespace StructureGo
{
    /// <summary>
    /// 
    /// </summary>
    public static class Graphviz
    {
        public const string LIB_GVC = @".\external\gvc.dll";
        public const string LIB_GRAPH = @".\external\cgraph.dll";
        public const int SUCCESS = 0;

        /// <summary>
        /// 创建一个Graphviz上下文
        /// </summary>
        /// <returns></returns>
        [DllImport(LIB_GVC, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr gvContext();

        /// <summary>
        /// 释放上下文的资源
        /// </summary>
        /// <param name="gvc"></param>
        /// <returns></returns>
        [DllImport(LIB_GVC, CallingConvention = CallingConvention.Cdecl)]
        private static extern int gvFreeContext(IntPtr gvc);

        /// <summary>
        /// 从字符串中读取图的配置
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [DllImport(LIB_GRAPH, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr agmemread(string data);


        /// <summary>
        /// 释放图的资源
        /// </summary>
        /// <param name="g"></param>
        [DllImport(LIB_GRAPH, CallingConvention = CallingConvention.Cdecl)]
        private static extern void agclose(IntPtr g);

        /// <summary>
        /// 设定图的布局
        /// </summary>
        /// <param name="gvc"></param>
        /// <param name="g"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        [DllImport(LIB_GVC, CallingConvention = CallingConvention.Cdecl)]
        private static extern int gvLayout(IntPtr gvc, IntPtr g, string engine);


        /// <summary>
        /// 释放布局资源
        /// </summary>
        /// <param name="gvc"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        [DllImport(LIB_GVC, CallingConvention = CallingConvention.Cdecl)]
        private static extern int gvFreeLayout(IntPtr gvc, IntPtr g);

        /// <summary>
        /// 渲染图像到文件当中
        /// </summary>
        /// <param name="gvc">上下文</param>
        /// <param name="g">图像</param>
        /// <param name="format">输出格式</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        [DllImport(LIB_GVC, CallingConvention = CallingConvention.Cdecl)]
        private static extern int gvRenderFilename(IntPtr gvc, IntPtr g,
              string format, string fileName);

        /// <summary>
        /// 渲染图像至内存中
        /// </summary>
        /// <param name="gvc"></param>
        /// <param name="g"></param>
        /// <param name="format"></param>
        /// <param name="result"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        [DllImport(LIB_GVC, CallingConvention = CallingConvention.Cdecl)]
        private static extern int gvRenderData(IntPtr gvc, IntPtr g,
              string format, out IntPtr result, out int length);

        /// <summary>
        /// 释放渲染资源
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        [DllImport(LIB_GVC, CallingConvention = CallingConvention.Cdecl)]
        private static extern int gvFreeRenderData(IntPtr result);

        /// <summary>
        /// 渲染图像到本地文件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="format"></param>
        /// <param name="fileName"></param>
        public static void RenderImageToFile(string source, string format, string fileName)
        {
            // 创建Graphviz上下文
            var gvc = gvContext();
            if (gvc == IntPtr.Zero)
                throw new Exception("Failed to create Graphviz context.");

            // 加载DOT数据至图中
            var g = agmemread(source);
            if (g == IntPtr.Zero)
                throw new Exception("Failed to create graph from source. Check for syntax errors.");

            // 加载一个布局
            if (gvLayout(gvc, g, "dot") != SUCCESS)
                throw new Exception("Layout failed.");

            // 渲染图
            if (gvRenderFilename(gvc, g, format, fileName) != SUCCESS)
                throw new Exception("Render failed.");
            gvFreeLayout(gvc, g);
            agclose(g);
            gvFreeContext(gvc);
        }

        public static MemoryStream RenderImage(string source, string format)
        {

            // 创建Graphviz上下文
            var gvc = gvContext();
            if (gvc == IntPtr.Zero)
                throw new Exception("Failed to create Graphviz context.");

            // 加载DOT数据至图中
            var g = agmemread(source);
            if (g == IntPtr.Zero)
                throw new Exception("Failed to create graph from source. Check for syntax errors.");

            // 加载一个布局
            if (gvLayout(gvc, g, "dot") != SUCCESS)
                throw new Exception("Layout failed.");

            // 渲染图
            if (gvRenderData(gvc, g, format, out var result, out var length) != SUCCESS)
                throw new Exception("Render failed.");

            // 将渲染的图放到内存当中
            var bytes = new byte[length];
            Marshal.Copy(result, bytes, 0, length);

            // 释放资源
            gvFreeRenderData(result);
            gvFreeLayout(gvc, g);
            agclose(g);
            gvFreeContext(gvc);
            var stream = new MemoryStream(bytes);
            return stream;
        }
    }
}
