using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;
using Microsoft.UI.Xaml.Media.Imaging;

namespace MyLittleWidget.Contracts.AppShortcut
{
  public sealed partial class AppShortcutContent : UserControl
  {
    // Ӧ��·������
    public string ApplicationPath { get; set; } = "";
    public string ApplicationArguments { get; set; } = "";
    public string IconPath { get; set; } = "";

    // ����¼�����֪ͨ�����
    public event EventHandler<AppShortcutEventArgs> SettingsRequested;
    public event EventHandler DeleteRequested;
    public event EventHandler EditRequested;

    public AppShortcutContent()
    {
      InitializeComponent();
      LoadDefaultIcon();
    }

    // ����Ӧ��
    private async void LaunchButton_Click(object sender, RoutedEventArgs e)
    {
      if (string.IsNullOrEmpty(ApplicationPath))
        return;

      try
      {
        if (ApplicationPath.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
        {
          // ������ͳ����Ӧ��
          var processStartInfo = new ProcessStartInfo
          {
            FileName = ApplicationPath,
            Arguments = ApplicationArguments,
            UseShellExecute = true
          };
          Process.Start(processStartInfo);
        }
        else
        {
          // ����UWPӦ�û�����Э��
          await Launcher.LaunchUriAsync(new Uri(ApplicationPath));
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine($"����Ӧ��ʧ��: {ex.Message}");
      }
    }

    // ���ò˵�����
    private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
    {
      SettingsRequested?.Invoke(this, new AppShortcutEventArgs
      {
        ApplicationPath = ApplicationPath,
        Arguments = ApplicationArguments,
        IconPath = IconPath
      });
    }

    // ѡ��ͼ��˵�����
    private async void SelectIconMenuItem_Click(object sender, RoutedEventArgs e)
    {
      // var picker = new FileOpenPicker();
      // var hwnd = WinRT.Interop.WindowNative.GetWindowHandle();
      // WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
      //
      // picker.ViewMode = PickerViewMode.Thumbnail;
      // picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
      // picker.FileTypeFilter.Add(".png");
      // picker.FileTypeFilter.Add(".jpg");
      // picker.FileTypeFilter.Add(".jpeg");
      // picker.FileTypeFilter.Add(".ico");
      // picker.FileTypeFilter.Add(".bmp");
      //
      // var file = await picker.PickSingleFileAsync();
      // if (file != null)
      // {
      //   IconPath = file.Path;
      //   SetAppIcon(IconPath);
      // }
    }

    // �༭�˵�����
    private void EditMenuItem_Click(object sender, RoutedEventArgs e)
    {
      EditRequested?.Invoke(this, EventArgs.Empty);
    }

    // ɾ���˵�����
    private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
    {
      DeleteRequested?.Invoke(this, EventArgs.Empty);
    }

    // ����Ӧ��ͼ��
    public void SetAppIcon(string iconPath)
    {
      if (!string.IsNullOrEmpty(iconPath) && System.IO.File.Exists(iconPath))
      {
        try
        {
          AppIconImage.Source = new BitmapImage(new Uri(iconPath));
          IconPath = iconPath;
        }
        catch
        {
          LoadDefaultIcon();
        }
      }
      else
      {
        LoadDefaultIcon();
      }
    }

    // ����Ĭ��ͼ��
    private void LoadDefaultIcon()
    {
      // ����Ĭ��ͼ�꣬����ʹ��ϵͳĬ��Ӧ��ͼ��
      AppIconImage.Source = new BitmapImage(
          new Uri("ms-appx:///Assets/AppIcon.png")); // ȷ�����������Դ
    }

    // ����Ӧ��·��
    public void SetApplicationPath(string path, string arguments = "")
    {
      ApplicationPath = path;
      ApplicationArguments = arguments;
    }
  }

  // �¼�������
  public class AppShortcutEventArgs : EventArgs
  {
    public string ApplicationPath { get; set; }
    public string Arguments { get; set; }
    public string IconPath { get; set; }
  }
}