using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GetSystemStatusGUI {
    public partial class NetworkForm : Form {
        private NetworkInfo networkInfo;
        private Chart[] subCharts;
        private Color baseColor = Color.LightCoral;
        private Color chartColor = Color.FromArgb(120, Color.LightCoral);
        private Color borderColor = Color.FromArgb(180, Color.LightCoral);
        private Color lineColor = Color.LightPink;
        private int beginTop;
        private int rows = 1, columns = 1;
        private const int history_length = 60;
        private const double margin_ratio = 35;
        private Form1 mainForm;

        public NetworkForm(Form1 mainForm, bool showVirtual = false) {
            InitializeComponent();
            networkInfo = new NetworkInfo(showVirtual);
            if (networkInfo.adapterNum == 0) {
                mainForm.DisableChecked("noNetwork");
                this.Dispose();
                return;
            }
            this.mainForm = mainForm;
        }

        private void NetworkForm_Load(object sender, EventArgs e) {
            List<int> y = new List<int>();
            for (int i = 0; i < history_length; i++) y.Add(0);
            beginTop = label1.Location.Y + label1.Size.Height;
            Utility.FactorDecompose(networkInfo.adapterNum, ref columns, ref rows);
            subCharts = new Chart[networkInfo.adapterNum];
            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < columns; j++) {
                    int cid = i * columns + j;
                    Chart chart = new Chart();
                    chart.Palette = ChartColorPalette.None;
                    chart.PaletteCustomColors = new Color[] { chartColor };
                    chart.Series.Add(cid.ToString());
                    chart.Series[0].Points.DataBindY(y);
                    chart.Series[0].ChartType = SeriesChartType.SplineArea;
                    chart.Series[0].BorderColor = borderColor;
                    chart.ChartAreas.Add(cid.ToString());
                    chart.ChartAreas[0].AxisY.Minimum = 0;
                    chart.ChartAreas[0].AxisY.Maximum = 100;
                    chart.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
                    chart.ChartAreas[0].AxisY.MajorTickMark.Enabled = false;
                    chart.ChartAreas[0].AxisY.MajorGrid.LineColor = lineColor;
                    chart.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
                    chart.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
                    chart.ChartAreas[0].AxisX.MajorGrid.LineColor = lineColor;
                    chart.ChartAreas[0].AxisX.MajorTickMark.Enabled = false;
                    chart.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
                    chart.ChartAreas[0].AxisX.MinorGrid.LineColor = lineColor;
                    chart.ChartAreas[0].AxisX.LineColor = baseColor;
                    chart.ChartAreas[0].AxisY.LineColor = baseColor;
                    chart.ChartAreas[0].AxisX.LineWidth = 2;
                    chart.ChartAreas[0].AxisY.LineWidth = 2;
                    chart.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
                    chart.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
                    chart.ChartAreas[0].AxisX2.Enabled = AxisEnabled.True;
                    chart.ChartAreas[0].AxisX2.LabelStyle.Enabled = false;
                    chart.ChartAreas[0].AxisX2.MajorGrid.Enabled = false;
                    chart.ChartAreas[0].AxisX2.MajorTickMark.Enabled = false;
                    chart.ChartAreas[0].AxisX2.LineColor = baseColor;
                    chart.ChartAreas[0].AxisX2.LineWidth = 2;
                    chart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
                    chart.ChartAreas[0].AxisY2.LabelStyle.Enabled = false;
                    chart.ChartAreas[0].AxisY2.MajorGrid.Enabled = false;
                    chart.ChartAreas[0].AxisY2.MajorTickMark.Enabled = false;
                    chart.ChartAreas[0].AxisY2.LineColor = baseColor;
                    chart.ChartAreas[0].AxisY2.LineWidth = 2;
                    chart.Titles.Add(cid.ToString() + "_0");
                    chart.Titles[0].Text = networkInfo.getAdapterName(cid);
                    chart.Titles[0].Alignment = ContentAlignment.MiddleLeft;
                    chart.Titles[0].DockedToChartArea = cid.ToString();
                    chart.Titles[0].IsDockedInsideChartArea = false;
                    chart.Titles[0].Font = new Font("微软雅黑", 13);
                    chart.Titles.Add(cid.ToString() + "_1");
                    //chart.Titles[1].Text = "Load rate in 60 secs";
                    chart.Titles[1].Text = networkInfo.getAdapterModel(cid);
                    chart.Titles[1].Alignment = ContentAlignment.MiddleLeft;
                    chart.Titles[1].DockedToChartArea = cid.ToString();
                    chart.Titles[1].IsDockedInsideChartArea = false;
                    chart.Titles[1].ForeColor = SystemColors.GrayText;
                    chart.Titles.Add(cid.ToString() + "_2");
                    chart.Titles[2].Text = "Upload 0bps\nDownload 0bps\nLink Speed 10Mbps\nIPv4 Address 127.0.0.1\nIPv6 Address ::1";
                    chart.Titles[2].Alignment = ContentAlignment.MiddleLeft;
                    chart.Titles[2].DockedToChartArea = cid.ToString();
                    chart.Titles[2].Docking = Docking.Bottom;
                    chart.Titles[2].IsDockedInsideChartArea = false;
                    chart.Titles[2].Font = new Font("微软雅黑", 11);
                    //chart.Titles[2].ForeColor = ColorTranslator.FromHtml("#494949");
                    subCharts[cid] = chart;
                    this.Controls.Add(subCharts[cid]);
                }
            }
            InitialSize();
            NetworkForm_Resize(null, null);
            new Action(network_load_thread).BeginInvoke(null, null);
        }

        private void NetworkForm_Resize(object sender, EventArgs e) {
            int marginHorizontal = (int)Math.Round((double)Math.Min(this.Size.Height, this.Size.Width) / (double)margin_ratio);
            int marginVertical = marginHorizontal;
            int endRight = (int)Math.Round((double)marginHorizontal * 1.1);
            int fixHeight = Math.Max(40, marginHorizontal * 2);
            int chartHeight = (int)Math.Round((double)(this.Size.Height - beginTop - fixHeight - (rows + 1) * marginVertical) / (double)rows);
            int chartWidth = (int)Math.Round((double)(this.Size.Width - endRight - (columns + 1) * marginHorizontal) / (double)columns);
            if (chartHeight <= 0 || chartWidth <= 0 || subCharts == null) return;
            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < columns; j++) {
                    int cid = i * columns + j;
                    int startX = (j + 1) * marginHorizontal + j * chartWidth;
                    int startY = beginTop + (i + 1) * marginVertical + i * chartHeight;
                    subCharts[cid].Size = new Size(chartWidth, chartHeight);
                    subCharts[cid].Location = new Point(startX, startY);
                }
            }
        }

        private void NetworkForm_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = true;
            mainForm.DisableChecked("Network");
        }

        private void InitialSize() {
            if (this.columns > 2)
                this.Width = (int)Math.Round(this.Width / 2f * columns * .97f);
        }

        private void NetworkForm_Deactivate(object sender, EventArgs e) {
            if (this.WindowState == FormWindowState.Minimized) {
                this.WindowState = FormWindowState.Normal;
                mainForm.DisableChecked("Network");
            }
        }

        private void network_load_thread() {
            List<float>[] ys = new List<float>[networkInfo.adapterNum];
            for (int i = 0; i < networkInfo.adapterNum; i++) {
                ys[i] = new List<float>();
                for (int j = 0; j < history_length; j++) ys[i].Add(0);
            }
            while (!subCharts[0].IsDisposed && !mainForm.IsDisposed) {
                float[] send_speed = new float[networkInfo.adapterNum];
                float[] receive_speed = new float[networkInfo.adapterNum];
                for (int i = 0; i < networkInfo.adapterNum; i++) {
                    float cLoad, cSendSpeed, cReceiveSpeed;
                    networkInfo.SpeedAndLoad(i, out cSendSpeed, out cReceiveSpeed, out cLoad);
                    ys[i].RemoveAt(0);
                    ys[i].Add(cLoad);
                    send_speed[i] = cSendSpeed;
                    receive_speed[i] = cReceiveSpeed;
                }
                Action updateChart = new Action(
                    delegate () {
                        for (int i = 0; i < networkInfo.adapterNum; i++) {
                            subCharts[i].Series[0].Points.DataBindY(ys[i]);
                            string titleStr = string.Empty;
                            string ud_spd = Utility.FormatSpeedString("Send", send_speed[i], "Receive", receive_speed[i], true);
                            string link_spd = "Link Speed " + networkInfo.getLinkSpeedString(i);
                            string ipv4_addr = "IPv4 Address " + networkInfo.getIPv4Address(i);
                            string ipv6_addr = "IPv6 Address " + networkInfo.getIPv6Address(i);
                            titleStr = ud_spd + "\n" + link_spd + "\n" + ipv4_addr + "\n" + ipv6_addr;
                            subCharts[i].Titles[2].Text = titleStr;
                        }
                    }
                );
                try { Invoke(updateChart); }
                catch { break; }
                Thread.Sleep(Global.interval_ms);
            }
        }
    }

    public class NetworkInfo {
        private NetworkInterface[] allAdapters;
        private List<NetworkInterface> validAdapters;
        private Dictionary<string, PerformanceCounter> pcNetworkReceive;
        private Dictionary<string, PerformanceCounter> pcNetworkSend;

        // 构造函数，初始化计数器
        public NetworkInfo(bool showVirtual = false) {
            //网卡性能计数器
            allAdapters = NetworkInterface.GetAllNetworkInterfaces();
            pcNetworkReceive = new Dictionary<string, PerformanceCounter>();
            pcNetworkSend = new Dictionary<string, PerformanceCounter>();
            foreach (NetworkInterface adapter in allAdapters) {
                if (Environment.OSVersion.Version.Major == 10) {
                    pcNetworkReceive.Add(adapter.Description, new PerformanceCounter("Network Adapter", "Bytes Received/sec", R(adapter.Description), "."));
                    pcNetworkSend.Add(adapter.Description, new PerformanceCounter("Network Adapter", "Bytes Sent/sec", R(adapter.Description), "."));
                } else {
                    pcNetworkReceive.Add(adapter.Description, new PerformanceCounter("Network Interface", "Bytes Received/sec", R(adapter.Description), "."));
                    pcNetworkSend.Add(adapter.Description, new PerformanceCounter("Network Interface", "Bytes Sent/sec", R(adapter.Description), "."));
                }
            }
            if (!showVirtual) FilterValidAdapters();
            else AddAllAdapters();
        }

        // 筛选出合法的网卡（活动的物理网卡）
        private void FilterValidAdapters() {
            validAdapters = new List<NetworkInterface>();
            foreach (NetworkInterface adapter in allAdapters) {
                if (adapter.Speed > 0 && adapter.Speed != 1073741824) {
                    string fRegistryKey = "SYSTEM\\CurrentControlSet\\Control\\Network\\{4D36E972-E325-11CE-BFC1-08002BE10318}\\" + adapter.Id + "\\Connection";
                    RegistryKey rk = Registry.LocalMachine.OpenSubKey(fRegistryKey, false);
                    if (rk == null) continue;
                    //区分 PnpInstanceID，前三个字符指示连接方式，PCI为PCI/PCIe内置网卡，USB为USB网卡，BTH为蓝牙网卡，ROOT为虚拟网卡；MediaSubType 为 01 则是虚拟网卡，02为无线网卡。
                    string fPnpInstanceID = rk.GetValue("PnpInstanceID", "").ToString();
                    int fMediaSubType = Convert.ToInt32(rk.GetValue("MediaSubType", 0));
                    if (fPnpInstanceID.Length <= 3) continue;
                    string connMethod = fPnpInstanceID.Substring(0, 3);
                    if ((connMethod == "PCI" || connMethod == "USB" || fMediaSubType == 2) && fMediaSubType != 1 && connMethod != "ROOT") {
                        this.validAdapters.Add(adapter);
                    } else if (connMethod == "BTH" && adapter.Speed != 3000000) {
                        this.validAdapters.Add(adapter);
                    }
                }
            }
        }

        // 添加所有网卡（启用了显示虚拟网络适配器）
        private void AddAllAdapters() {
            validAdapters = new List<NetworkInterface>();
            foreach (NetworkInterface adapter in allAdapters) {
                if (adapter.Speed > 0 && adapter.Speed != 1073741824) {
                    this.validAdapters.Add(adapter);
                }
            }
        }

        // 网卡数量
        public int adapterNum {
            get { return this.validAdapters.Count; }
        }

        // 网卡类型（以太网/Wi-Fi/蓝牙网络连接）
        public string getNetworkInterfaceType(int id) {
            return this.validAdapters[id].NetworkInterfaceType.ToString();
        }

        // 网络名称
        public string getAdapterName(int id) {
            return this.validAdapters[id].Name;
        }

        // 网卡型号
        public string getAdapterModel(int id) {
            return validAdapters[id].Description;
        }

        // 网卡链接速度
        public string getLinkSpeedString(int id) {
            NetworkInterface adapter = validAdapters[id];
            long linkSpeed = adapter.Speed / 1000 / 1000;
            string nScale = "Mbps";
            if (linkSpeed >= 1000) {
                linkSpeed /= 1000;
                nScale = "Gbps";
            }
            return linkSpeed + " " + nScale;
        }
        // 单位：bps
        public long getLinkSpeed(int id) {
            NetworkInterface adapter = validAdapters[id];
            long linkSpeed = adapter.Speed;
            return linkSpeed;
        }

        // IPv4地址
        public string getIPv4Address(int id) {
            NetworkInterface adapter = validAdapters[id];
            IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
            UnicastIPAddressInformationCollection UnicastIPAddressInformationCollection = adapterProperties.UnicastAddresses;
            string IPv4Adrress = "Not Present";
            foreach (UnicastIPAddressInformation UnicastIPAddressInformation in UnicastIPAddressInformationCollection) {
                if (UnicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                    IPv4Adrress = UnicastIPAddressInformation.Address.ToString();
            }
            return IPv4Adrress;
        }

        // IPv6地址
        public string getIPv6Address(int id) {
            NetworkInterface adapter = validAdapters[id];
            IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
            UnicastIPAddressInformationCollection UnicastIPAddressInformationCollection = adapterProperties.UnicastAddresses;
            string IPv6Address = "Not Present";
            foreach (UnicastIPAddressInformation UnicastIPAddressInformation in UnicastIPAddressInformationCollection) {
                if (UnicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetworkV6) {
                    IPv6Address = UnicastIPAddressInformation.Address.ToString();
                    break;
                }
            }
            return IPv6Address;
        }

        // 网卡上传、下载速率
        // 单位：bps
        public float ReceiveSpeed(int id) {
            return ReceiveSpeed(validAdapters[id].Description);
        }
        public float SendSpeed(int id) {
            return SendSpeed(validAdapters[id].Description);
        }
        public float ReceiveSpeed(string AdapterDesc) {
            PerformanceCounter selectedPC;
            if (!pcNetworkReceive.TryGetValue(AdapterDesc, out selectedPC)) return -1;
            return selectedPC.NextValue() * 8;
        }
        public float SendSpeed(string AdapterDesc) {
            PerformanceCounter selectedPC;
            if (!pcNetworkSend.TryGetValue(AdapterDesc, out selectedPC)) return -1;
            return selectedPC.NextValue() * 8;
        }

        // 网卡负载率
        // 计算方法：(上行带宽+下行带宽)/链接速率*100%
        public float AdapterLoad(int id) {
            float upspd = this.SendSpeed(id);
            float dwnspd = this.ReceiveSpeed(id);
            long linkSpeed = this.getLinkSpeed(id);
            float load = (upspd + dwnspd) * 100 / linkSpeed;
            return load;
        }

        public void SpeedAndLoad(int id, out float sendSpeed, out float receiveSpeed, out float adapterLoad) {
            float upspd = this.SendSpeed(id);
            float dwnspd = this.ReceiveSpeed(id);
            long linkSpeed = this.getLinkSpeed(id);
            float load = (upspd + dwnspd) * 100 / linkSpeed;
            sendSpeed = upspd;
            receiveSpeed = dwnspd;
            adapterLoad = load;
        }

        private string R(string str) {
            string ret = str.Replace('#', '_').Replace('(', '[').Replace(')', ']').Replace('/', '_');
            if (ret.Contains("Microsoft ISATAP Adapter")) ret = "isatap.localdomain";
            return ret;
        }
    }
}