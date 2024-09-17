namespace XmobiTea.ProtonNet.Server.WebApi.Controllers.Render
{
    class Constance
    {
        public static readonly string RenderPartialPattern = @"@renderPartial\(\s*""([^""]+)""\s*\);";
        public static readonly string RenderLayoutPattern = @"@renderLayout\(\s*""([^""]+)""\s*\);";
        public static readonly string RenderSessionPattern = @"@renderSession\(\s*""([^""]+)""\s*\);";
        public static readonly string RenderViewDataPattern = @"@renderViewData\(\s*""([^""]+)""\s*\);";
        public static readonly string SetViewDataPattern = @"@setViewData\(\s*""([^""]+)""\s*,\s*([^)]+)\s*\);";

        public static readonly string SessionPattern = @"@session\s+(\w+)\s*{([^}]*)}";
        public static readonly string InitPattern = @"@pinit\s*{([^}]*)}";

        public static readonly string IfPattern = @"@if\s*\(\s*(.+?)\s*\)\s*{([^}]*)}";
        public static readonly string IfElsePattern = @"@if\s*\(\s*(.+?)\s*\)\s*{([^}]*)}\s*@else\s*{([^}]*)}";
        public static readonly string ForeachPattern = @"@foreach\s*\(@var\s+(\w+)\s+in\s+@getViewData\(""(\w+)""\)\)\s*{([^}]*)}";

    }

}
