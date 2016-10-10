using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xiaogongju
{
    public partial class Xiaogongju : Form
    {
        HttpClient web;
        public Byte[] codeImageData;
        public Xiaogongju()
        {
            InitializeComponent();
        }

        private void Xiaogongju_Load(object sender, EventArgs e)
        {
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void textBox3_TextChanged() { 
        }
        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            string URL = "http://gsxt.gzgs.gov.cn/search!generateCode.shtml";
            URL += "?validTag=searchImageCode";
            web = new HttpClient();
            try {
                codeImageData = web.DownloadData(URL);
                pictureBox1.Image = Bitmap.FromStream(new MemoryStream(codeImageData));
                this.ocr_code();
            }catch (WebException err){
                MessageBox.Show(err.InnerException.Message);
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string url = "http://gsxt.gzgs.gov.cn/query!searchSczt.shtml";
            StringBuilder postData = new StringBuilder();
            postData.Append("q=1&validCode=" + textBox1.Text);
            byte[] sendData = Encoding.GetEncoding("UTF-8")
                                .GetBytes(postData.ToString());
            web.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            web.Headers.Add("ContentLength", sendData.Length.ToString());
            byte[] recData = web.UploadData(url, "POST", sendData);
            MessageBox.Show(Encoding.GetEncoding("UTF-8").GetString(recData));
        }

        private void ocr_code()
        {
            string url = "http://apis.baidu.com/idl_baidu/baiduocrpay/idlocrpaid";
            StringBuilder postData = new StringBuilder();
            postData.Append("fromdevice=pc&clientip=10.10.10.0&detecttype=LocateRecognize"
                            +"&languagetype=CHN_ENG&imagetype=1");
            string base64String = Convert.ToBase64String(codeImageData);
            Console.Write(base64String);
            postData.Append("&image=" + base64String);
            byte[] sendData = Encoding.GetEncoding("UTF-8")
                                .GetBytes(postData.ToString());
            web.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            web.Headers.Add("ContentLength", sendData.Length.ToString());
            web.Headers.Add("apikey", "key");
            byte[] recData = web.UploadData(url, "POST", sendData);
            //上传完成
            //获取json数据
            string jsonString;
            textBox2.Text = 
                jsonString = Encoding.GetEncoding("UTF-8")
                    .GetString(recData);
            //MessageBox.Show(jsonString);
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                ((TextBox)sender).SelectAll();
            } 
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
    class HttpClient : WebClient
    {
        // Cookie 容器
        private CookieContainer cookieContainer;

        /**/
        /// <summary>
        /// 创建一个新的 WebClient 实例。
        /// </summary>
        public HttpClient()
        {
            this.cookieContainer = new CookieContainer();
        }

        /**/
        /// <summary>
        /// 创建一个新的 WebClient 实例。
        /// </summary>
        /// <param name="cookie">Cookie 容器</param>
        public HttpClient(CookieContainer cookies)
        {
            this.cookieContainer = cookies;
        }

        /**/
        /// <summary>
        /// Cookie 容器
        /// </summary>
        public CookieContainer Cookies
        {
            get { return this.cookieContainer; }
            set { this.cookieContainer = value; }
        }

        /**/
        /// <summary>
        /// 返回带有 Cookie 的 HttpWebRequest。
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                HttpWebRequest httpRequest = request as HttpWebRequest;
                httpRequest.CookieContainer = cookieContainer;
            }
            return request;
        }
    }
}
