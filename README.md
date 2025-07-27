# Community.Widget

Community.Widget 是一个小组件平台项目，允许第三方开发者创建和集成自己的桌面小组件。该项目基于 WinUI 3 构建，提供了可扩展的组件框架。

## 项目结构

```
Community.Widget/
├── Community.Widget
│   ├── Properties/
│   │   └── launchSettings.json
│   ├── App.xaml.cs
│   ├── MainWindow.xaml.cs        # 主应用程序(用于显示小组件)
│   └── CommunityApplicationSettings.cs
├── MyLittleWidget.Contracts      # 组件开发契约库
│   ├── AppShortcut/              # 示例组件：应用程序快捷方式
│   │   ├── AppShortcut.cs
│   │   └── AppShortcutContent.xaml.cs
│   ├── OneLineOfWisdom/          # 示例组件：每日一句
│   │   └── OneLineOfWisdom.cs
│   ├── PomodoroClock/            # 示例组件：番茄钟
│   │   ├── PomodoroClock.cs
│   │   └── PomodoroViewModel.cs
│   ├── WidgetBase.cs             # 组件基类
│   ├── WidgetConfig.cs           # 组件配置类
│   ├── IApplicationSettings.cs   # 应用程序设置接口
│   ├── IWidgetToolService.cs     # 工具服务接口
│   └── CustomWidget.cs           # 自定义组件教程
└── README.md
```

## 快速开始

### 1. 创建自定义组件

要创建自己的小组件，您需要继承 `WidgetBase` 类。以下是创建简单组件的示例：

```csharp
using MyLittleWidget.Contracts;
using Microsoft.UI.Xaml.Controls;

public class MyCustomWidget : WidgetBase
{
    public MyCustomWidget(WidgetConfig config, IApplicationSettings settings) 
        : base(config, settings)
    {
        // 设置组件内容
        if (Content is Border border)
        {
            border.Child = new TextBlock 
            { 
                Text = "Hello, World!",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }
        
        // 可以设置组件的默认大小（以单位格数）
        Config.UnitWidth = 2;
        Config.UnitHeight = 2;
    }
}
```

如果需要使用工具服务，可以使用带 [IWidgetToolService](file://c:\Users\123\Desktop\Community.Widget\MyLittleWidget.Contracts\IWidgetToolService.cs#L4-L45) 参数的构造函数：

```csharp
public class MyCustomWidget : WidgetBase
{
    public MyCustomWidget(WidgetConfig config, IApplicationSettings settings, IWidgetToolService toolService) 
        : base(config, settings, toolService)
    {
        // 设置组件内容
        if (Content is Border border)
        {
            border.Child = new TextBlock 
            { 
                Text = "Hello, World!",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }
    }
}
```

**提示:** 组件内容的设置可以选择直接用 C# 代码实现，也可以使用 XAML，甚至是 WebView。

个人建议使用 `border.Child = new TextBlock()` 而不是 `Content = new TextBlock()`。

### 2. 组件配置

每个组件都需要一个 [WidgetConfig](file://c:\Users\123\Desktop\Community.Widget\MyLittleWidget.Contracts\WidgetConfig.cs#L4-L21) 实例来配置组件的基本属性：

```csharp
var config = new WidgetConfig
{
    Id = 1,
    Name = "My Custom Widget",
    UnitWidth = 2,
    UnitHeight = 2,
    PositionX = 100,
    PositionY = 100
};
```

### 3. 使用应用程序设置

通过 [IApplicationSettings](file://c:\Users\123\Desktop\Community.Widget\MyLittleWidget.Contracts\IApplicationSettings.cs#L4-L12) 接口，您可以访问应用程序的全局设置：

```csharp
// 访问基础单位大小
var baseUnit = AppSettings.BaseUnit;

// 检查是否为深色主题
var isDark = AppSettings.IsDarkTheme;
```

组件会自动响应这些设置的变化。

### 4. 使用工具服务

通过 [IWidgetToolService](file://c:\Users\123\Desktop\Community.Widget\MyLittleWidget.Contracts\IWidgetToolService.cs#L4-L44) 接口，您可以访问应用程序提供的工具功能：

```csharp
// 选择文件或文件夹
var file = await ToolService.PickFileOrFolderAsync(false, ".txt", ".pdf");

// 显示简单通知
await ToolService.ShowNotificationAsync("标题", "这是一条通知消息");

// 显示交互式通知
await ToolService.ShowInteractiveNotificationAsync(
    "设置时间", 
    "请选择专注时间",
    "timeSelector",
    new [] { ("10", "10分钟"), ("15", "15分钟"), ("30", "30分钟") },
    "15",
    "customTime",
    "自定义时间(分钟)",
    "确定",
    ("action", "setTime"));
```

## 核心概念

### WidgetBase 类

[WidgetBase](file://c:\Users\123\Desktop\Community.Widget\MyLittleWidget.Contracts\WidgetBase.cs#L10-L225) 是所有组件的基类，提供了以下核心功能：

- 位置管理与拖拽支持
- 主题适配（深色/浅色模式）
- 基于单位的尺寸系统
- 全局设置变更通知
- 工具服务访问

### WidgetConfig 类

[WidgetConfig](file://c:\Users\123\Desktop\Community.Widget\MyLittleWidget.Contracts\WidgetConfig.cs#L4-L21) 用于配置组件的基本属性：

- [Id](file://c:\Users\123\Desktop\Community.Widget\MyLittleWidget.Contracts\WidgetConfig.cs#L6-L6): 组件唯一标识符
- [Name](file://c:\Users\123\Desktop\Community.Widget\MyLittleWidget.Contracts\WidgetConfig.cs#L7-L7): 组件名称
- `UnitWidth/UnitHeight`: 组件占用的网格单位
- `PositionX/PositionY`: 组件在画布上的位置

### IApplicationSettings 接口

通过 [IApplicationSettings](file://c:\Users\123\Desktop\Community.Widget\MyLittleWidget.Contracts\IApplicationSettings.cs#L4-L12) 接口，组件可以访问应用程序级别的设置：

- [BaseUnit](file://c:\Users\123\Desktop\Community.Widget\MyLittleWidget.Contracts\IApplicationSettings.cs#L7-L7): 基础单位大小，影响所有组件的尺寸
- [IsDarkTheme](file://c:\Users\123\Desktop\Community.Widget\MyLittleWidget.Contracts\IApplicationSettings.cs#L10-L10): 当前主题模式

### IWidgetToolService 接口

[IWidgetToolService](file://c:\Users\123\Desktop\Community.Widget\MyLittleWidget.Contracts\IWidgetToolService.cs#L4-L44) 提供了常用的工具方法：

- 文件/文件夹选择器
- 通知显示功能
- 交互式通知功能

## 开发指南

### 1. 继承 WidgetBase

创建组件的第一步是继承 [WidgetBase](file://c:\Users\123\Desktop\Community.Widget\MyLittleWidget.Contracts\WidgetBase.cs#L10-L225) 类并实现构造函数：

```csharp
public class MyWidget : WidgetBase
{
    public MyWidget(WidgetConfig config, IApplicationSettings settings) 
        : base(config, settings)
    {
        // 初始化组件
    }
    
    // 或者如果需要使用工具服务
    public MyWidget(WidgetConfig config, IApplicationSettings settings, IWidgetToolService toolService) 
        : base(config, settings, toolService)
    {
        // 初始化组件
    }
}
```

### 2. 配置组件内容

在构造函数中配置组件的视觉内容：

```csharp
protected override void ConfigureWidget()
{
    if (Content is Border border)
    {
        border.Child = new MyWidgetView(); // 设置组件的用户界面
    }
}
```

### 3. 响应主题变化

重写 [UpdateTheme](file://c:\Users\123\Desktop\Community.Widget\MyLittleWidget.Contracts\WidgetBase.cs#L124-L128) 方法以支持主题切换：

```csharp
protected override void UpdateTheme(bool isDark)
{
    base.UpdateTheme(isDark);
    // 添加自定义主题逻辑
}
```

### 4. 使用工具服务

如果组件构造时传入了 [IWidgetToolService](file://c:\Users\123\Desktop\Community.Widget\MyLittleWidget.Contracts\IWidgetToolService.cs#L4-L45)，可以通过 `ToolService` 属性访问：

```csharp
// 在组件类中
private async Task SelectFile()
{
    var file = await ToolService.PickFileOrFolderAsync();
    // 处理选中的文件
}
```

## 示例组件

项目中包含了几个示例组件：

1. **AppShortcut** - 应用程序快捷方式组件
2. **OneLineOfWisdom** - 显示每日一句的组件
3. **PomodoroClock** - 番茄工作法计时器（使用了 IWidgetToolService）
4. **CustomWidget** - 最基本的组件教程

您可以参考这些示例来创建自己的组件。

## 贡献

欢迎贡献新的组件或改进现有功能。要贡献代码，请遵循以下步骤：

1. Fork 项目
2. 创建功能分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 开启 Pull Request
