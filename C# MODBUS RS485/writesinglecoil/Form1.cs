using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Collections;

//using IndustrialNetworks.ModbusCore.ASCII;
//using Conversion;

namespace writesinglecoil
{
    public partial class Form1 : Form
    {
        private const char Header = ':';
        private const char Trailer = '\r';
        public delegate void MyEventHandler(object sender, EventArgs e);

        public event MyEventHandler MyEvent;
        byte slaveId = 01;
        ushort startAddress = 2049;//2049
        ushort startAddress3 = 2052;
        ushort startAddress4 = 2054;
        
        ushort startAddress2 = 2056;//m8
        ushort startAddress5 = 2050;//m2
        byte func = 05;
        private SerialPort serialPort1=null;
        int t1 = 0;
        int t2 = 0;
        int t3 = 0;
        int t4 = 0;
        int t5 = 0;
        int la1 = 0, dem1 = 0, dem2 = 0, dem3 = 0, dem4 = 0, dem5 = 0;// dem la bien tich 60s , la la bien hien tren min , t la nhan biet cua 
        int la2 = 0, nhan1 = 0, nhan2 = 0, nhan3 = 0, nhan4 = 0, nhan5 = 0;// nhan la bien tu waiting ve running 
        int la3 = 0, mark1 = 0, mark2 = 0, mark3 = 0, mark4 = 0; // mark la bien de biet danh dau va bo danh dau
        int la4 = 0, la5 = 0;
         int long1 = 0;
         //private void timer2_Tick(object sender, EventArgs e);
         int long2 = 0;// long1, long2 la2 bien tu so may va so lan
        // object sender1, sender2, sender3, sender4;
        // EventArgs e1, e2, e3, e4;
       

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            

            
            //SerialPort serialPort1;
            



        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //SerialPort serialPort1;
            try
            {
                serialPort1 = new SerialPort("COM9", 9600, Parity.Even,7,StopBits.One);
                serialPort1.Handshake = Handshake.None;
                serialPort1.Open();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

            if (serialPort1.IsOpen) serialPort1.Close();
            serialPort1.Open();
        }
        private String write(byte slaveAddress, ushort startAddress, byte functionCode, bool value)
        {
            string ON = "FF00";
            string OFF = "0000";
            string frame = string.Format("{0:X2}", slaveAddress);
            frame += string.Format("{0:X2}", functionCode);
            frame += string.Format("{0:X4}", startAddress);
            frame += string.Format("{0:X4}", (value? ON:OFF));
            byte[] bytes = HexToBytes(frame);
            byte lrc = LRC(bytes);
            return Header + frame + lrc.ToString("X2") + Trailer;


        }
        private byte[] HexToBytes(string hex)
        {
            if (hex == null)
                throw new ArgumentNullException("The data is null");

            if (hex.Length % 2 != 0)
                throw new FormatException("Hex Character Count Not Even");

            byte[] bytes = new byte[hex.Length / 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                //long1 = hex.Substring(i * 2, 3);
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }

            return bytes;
        }

        private byte LRC(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("tham so truyen vao ko ton tai phan tu nao");
            byte lrc = 0;
            foreach (byte b in data)
                lrc += b;
            lrc = (byte)((lrc ^ 0xFF) + 1);
            return lrc;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool ON = true;
            string frame = this.write(slaveId, startAddress, func, ON);
            serialPort1.WriteLine(frame);
            //textBox41.Text = frame; 
            Thread.Sleep(10);
            bool OFF = false;
            string frame1 = this.write(slaveId, startAddress, func, OFF);//2049
            serialPort1.WriteLine(frame1);
            Thread.Sleep(10);
            //Y0.Text = "RUN";
           // Y0.BackColor = Color.Lime;
            //Y0.ForeColor = Color.Black;
            //break;
           
        }
        public void button1Click(object sender, EventArgs e)
        {
            bool ON = true;
            string frame = this.write(slaveId, startAddress, func, ON);
            serialPort1.WriteLine(frame);
            Thread.Sleep(10);
            bool OFF = false;
            string frame1 = this.write(slaveId, startAddress, func, OFF);//2049
            serialPort1.WriteLine(frame1);
            Thread.Sleep(10);
            //Y0.Text = "RUN";
            // Y0.BackColor = Color.Lime;
            //Y0.ForeColor = Color.Black;
            //break;

        }

        

       

       
        private void button13_Click(object sender, EventArgs e)
        {
            

            //int tam;
            long1 = Convert.ToInt32(textBox1.Text); // so lan
            long2 = Convert.ToInt32(textBox2.Text); // so may
            //textBox2 = long1;
            //button1_Click(sender, e);
            //button1_Click(sender, e);
            //button1_Click(sender, e);
            if (long2 == 1)
            {
               

                Thread thrd = new Thread(ghi1);
               
                thrd.IsBackground = true;
                thrd.Start();
                timer2.Start();
               
                
            }
            else if (long2 == 2)
            {
                Thread thrd2 = new Thread(ghi2);
                thrd2.IsBackground = true;
                thrd2.Start();
                timer3.Start();
                
            }
            else if (long2 == 3)
            {
                Thread thrd3 = new Thread(ghi3);
                thrd3.IsBackground = true;
                thrd3.Start();
                timer4.Start();

               
            }
            else if (long2 == 4)
            {
                Thread thrd4 = new Thread(ghi4);
                thrd4.IsBackground = true;
                thrd4.Start();
                timer5.Start();

                
            }
            else if (long2 == 5)
            {
                if (long1 > 3 || long1 <= 0)
                {
                    MessageBox.Show("nhap sai so du vao");
                }
                else
                {
                    Thread thrd5 = new Thread(ghi5);
                    thrd5.IsBackground = true;
                    thrd5.Start();
                    timer6.Start();

                }
                
            }
            else
            {
                MessageBox.Show("nhap sai so du vao");
            }

        }
        
        void ghi1()
        {
            int tam;
           
            la1 = long1 * 10;
            for (tam = 0; tam < long1; tam++)
            {
                //=new System.EventHandler(this.button1_Click);
                //button1_Click();
                //button1Click(sender, e);
                //Thread.Sleep(420);
                // button2_Click(sender, e);
                // Thread.Sleep(30);
                bool ON = true;
                //bool OFF = false;
                string frame = this.write(slaveId, startAddress, func, ON);
                textBox41.Text = frame;
                //thrdd.Start();
                serialPort1.WriteLine(frame);
                //thrdd.Abort();
                //Thread.Sleep(10);
                //bool OFF = false;
               // string frame1 = this.write(slaveId, startAddress, func, OFF);//2049
               // serialPort1.WriteLine(frame1);
                //textBox41.Text = frame1;

                Thread.Sleep(400);


            }
            k1.Value = long1 * 600;
            dem1 = 0;
            lam1.Text = " Start running";
            l1.Text = la1.ToString();
           // timer2.Start();

        }
        void ghi2()
        {
            int tam;
            la2 = long1 * 10;
            for (tam = 0; tam < long1; tam++)
            {
                //=new System.EventHandler(this.button1_Click);
                //button1_Click();
                //button5Click(sender, e);
                bool ON = true;
                string frame = this.write(slaveId, startAddress5, func, ON);
                serialPort1.WriteLine(frame);
                textBox42.Text = frame;
                Thread.Sleep(400);
                //bool OFF = false;
                //string frame1 = this.write(slaveId, startAddress2, func, OFF);//2049
                //serialPort1.WriteLine(frame1);
                //Thread.Sleep(10);
                //Thread.Sleep(20);
                // button2_Click(sender, e);
                // Thread.Sleep(30);


            }
            k2.Value = long1 * 600;
            dem2 = 0;
            lam2.Text = "Start running";
            l2.Text = la2.ToString();
            //timer3.Start();

        }
        void ghi3()
        {
            int tam;
            la3 = long1 * 10;
            for (tam = 0; tam < long1; tam++)
            {
                //=new System.EventHandler(this.button1_Click);
                //button1_Click();
                //button6Click(sender, e);
                bool ON = true;
                string frame = this.write(slaveId, startAddress3, func, ON);
                serialPort1.WriteLine(frame);
                textBox43.Text = frame;
                Thread.Sleep(400);
                //bool OFF = false;
               // string frame1 = this.write(slaveId, startAddress3, func, OFF);//2049
                //serialPort1.WriteLine(frame1);
                //Thread.Sleep(10);
                //Thread.Sleep(20);
                // button2_Click(sender, e);
                // Thread.Sleep(30);


            }
            k3.Value = long1 * 600;
            dem3 = 0;
            lam3.Text = "Start running";
            l3.Text = la3.ToString();
           // timer4.Start();
        }
        void ghi4()
        {
            int tam;
            la4 = long1 * 10;
            for (tam = 0; tam < long1; tam++)
            {
                //=new System.EventHandler(this.button1_Click);
                //button1_Click();
                //button7Click(sender, e);
                bool ON = true;
                string frame = this.write(slaveId, startAddress4, func, ON);
                textBox44.Text = frame;
                serialPort1.WriteLine(frame);

                Thread.Sleep(400);
                //bool OFF = false;
                //string frame1 = this.write(slaveId, startAddress4, func, OFF);//2049
                //serialPort1.WriteLine(frame1);
                //Thread.Sleep(10);
                //Thread.Sleep(20);
                // button2_Click(sender, e);
                // Thread.Sleep(30);


            }
            k4.Value = long1 * 600;
            dem4 = 0;
            lam4.Text = "Start running";
            l4.Text = la4.ToString();
            //timer5.Start();
        }
        void ghi5()
        {
            int tam;
            la5 = long1 * 10;
            for (tam = 0; tam < long1; tam++)
            {
                //=new System.EventHandler(this.button1_Click);
                //button1_Click();
                //button7Click(sender, e);
                bool ON = true;
                string frame = this.write(slaveId, startAddress5, func, ON);
                //textBox45.Text = frame;
                serialPort1.WriteLine(frame);
                Thread.Sleep(400);
                bool OFF = false;
                string frame1 = this.write(slaveId, startAddress5, func, OFF);//2049
                serialPort1.WriteLine(frame1);
                //Thread.Sleep(10);
                Thread.Sleep(10);
                // button2_Click(sender, e);
                // Thread.Sleep(30);


            }
            k5.Value = long1 * 600;
            dem5 = 0;
            lam5.Text = "Start running";
            l5.Text = la5.ToString();
            //timer5.Start();
        }
        

         void button9_Click(object sender, EventArgs e)
        {
            Thread thrdd = new Thread(doc);
            thrdd.IsBackground = true;
            thrdd.Start();
            timer1.Start();
        }
        void doc()
        {
           
                try
                {
                    byte slaveAddress = 01;
                    byte function = 01;
                    uint startAddress = 0;
                    uint numberOfPoints = 8;

                    string frame = this.Read(slaveAddress, startAddress, function, numberOfPoints);
                    //textBox41.Text = frame;
                    if (serialPort1.IsOpen)
                    {

                        serialPort1.WriteLine(frame);

                        Thread.Sleep(100);


                        string bufferReceiver = serialPort1.ReadLine();
                        //textBox42.Text = bufferReceiver;
                        //bufferReceiver = serialPort1.ReadLine();
                        //bufferReceiver = serialPort1.ReadLine();
                        //bufferReceiver = serialPort1.ReadLine();
                        //textBox2.Text = bufferReceiver;
                        //textBox3.Text = "ok";
                        string tempStrg = bufferReceiver.Substring(1, bufferReceiver.Length - 2);
                        //textBox42.Text = tempStrg;
                        //Substring(1, bufferReceiver.Length - 2);
                        //textBox3.Text = serialPort1.IsOpen;
                        byte[] messageReceived = HexToBytes(tempStrg);
                        //textBox42.Text = messageReceived;

                        //if (bufferReceiver.Length == 10) CheckValidate(messageReceived);
                        byte[] data = new byte[messageReceived[2]];
                        Array.Copy(messageReceived, 3, data, 0, data.Length);
                        serialPort1.DiscardInBuffer();

                        bool[] temp = ByteToBool(data);
                        //textBox42.Text = temp;
                        string result = string.Empty;
                        string result0 = string.Empty;
                        string result1 = string.Empty;
                        string result2 = string.Empty;
                        string result3 = string.Empty;
                        string result4 = string.Empty;
                        string result5 = string.Empty;
                        string result6 = string.Empty;
                        string result7 = string.Empty;
                        foreach (var item in temp)
                        {
                            result += string.Format("{0} ", item);
                            // result1 = string.Format("{0} ", item);
                            // result1 = string.Format("{0} ", item);

                        }
                        //textBox43.Text = result;
                        //textBox4.Text = result;
                        result0 = string.Format("{0} ", temp[0]);
                        result1 = string.Format("{0} ", temp[1]);
                        result2 = string.Format("{0} ", temp[2]);
                        result3 = string.Format("{0} ", temp[3]);
                        result4 = string.Format("{0} ", temp[4]);
                        //result5 = string.Format("{0} ", temp[5]);
                        //result6 = string.Format("{0} ", temp[6]);
                        //result7 = string.Format("{0} ", temp[7]);
                        ///x0
                        if (result0 == "True ")
                        {
                            X0.Text = "ON";
                            X0.BackColor = Color.Lime;
                            X0.ForeColor = Color.Black;
                            t1 = 1;// cua dong
                        }
                        else if (result0 == "False ")
                        {
                            X0.Text = "OFF";
                            X0.BackColor = Color.Red;
                            X0.ForeColor = Color.White;
                            t1 = 0;
                        }
                        else
                        {
                            X0.Text = "ko co";
                        }
                        // x1
                        if (result4 == "True ")
                        {
                            X1.Text = "ON";
                            X1.BackColor = Color.Lime;
                            X1.ForeColor = Color.Black;
                            t2 = 1;
                        }
                        else if (result4 == "False ")
                        {
                            X1.Text = "OFF";
                            X1.BackColor = Color.Red;
                            X1.ForeColor = Color.White;
                            t2 = 0;
                        }
                        else
                        {
                            X1.Text = "ko co";
                        }
                        //x2
                        if (result2 == "True ")
                        {
                            X2.Text = "ON";
                            X2.BackColor = Color.Lime;
                            X2.ForeColor = Color.Black;
                            t3 = 1;
                        }
                        else if (result2 == "False ")
                        {
                            X2.Text = "OFF";
                            X2.BackColor = Color.Red;
                            X2.ForeColor = Color.White;
                            t3 = 0;
                        }
                        else
                        {
                            X2.Text = "ko co";
                        }
                        //x3
                        if (result3 == "True ")
                        {
                            X3.Text = "ON";
                            X3.BackColor = Color.Lime;
                            X3.ForeColor = Color.Black;
                            t4 = 1;
                        }
                        else if (result3 == "False ")
                        {
                            X3.Text = "OFF";
                            X3.BackColor = Color.Red;
                            X3.ForeColor = Color.White;
                            t4 = 0;
                        }
                        else
                        {
                            X3.Text = "ko co";
                        }
                        //x4
                        if (result1 == "True ")
                        {
                            X4.Text = "ON";
                            X4.BackColor = Color.Lime;
                            X4.ForeColor = Color.Black;
                            t5 = 1;
                        }
                        else if (result1 == "False ")
                        {
                            X4.Text = "OFF";
                            X4.BackColor = Color.Red;
                            X4.ForeColor = Color.White;
                            t5 = 0;
                        }
                        else
                        {
                            X4.Text = "ko co";
                        }

                        // timer1.Start();





                    }

                }
                catch (Exception ex)
                {
                    //textBox4.Text = ex.Message;

                }
            
        }

         private bool[] ByteToBool(byte[] value)
        {
            List<bool> result = new List<bool>();
            BitArray bits = new BitArray(value);
            //int i=0;
            for (int i = 0; i < bits.Count; i++) 
            {
                result.Add(bits[i]);
            }
            return result.ToArray();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            button9_Click(sender, e);
        }
         private string Read(byte slaveAddress, uint startAddress, byte functionCode, uint numberOfPoints)
        {
            string frame = string.Format("{0:X2}", slaveAddress);
            frame += string.Format("{0:X2}", functionCode);
            frame += string.Format("{0:X4}", startAddress);
            frame += string.Format("{0:X4}", numberOfPoints);
            byte[] bytes = HexToBytes(frame);
            byte lrc = LRC(bytes);
            //textBox3.Text = string.Format("{0:X2}", lrc);
            return Header + frame + lrc.ToString("X2") + Trailer;
        }

         private void label1_Click(object sender, EventArgs e)
         {

         }

         private void textBox1_TextChanged(object sender, EventArgs e)
         {

         }

        private void lam1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)// MO MAY 2
         {
             bool ON = true;
             string frame = this.write(slaveId, startAddress2, func, ON);
             serialPort1.WriteLine(frame);
             Thread.Sleep(10);
             bool OFF = false;
             string frame1 = this.write(slaveId, startAddress2, func, OFF);//2049
             serialPort1.WriteLine(frame1);
             Thread.Sleep(10);
         }

         private void button6_Click(object sender, EventArgs e)// mo may 3
         {
             bool ON = true;
             string frame = this.write(slaveId, startAddress3, func, ON);
             serialPort1.WriteLine(frame);
             Thread.Sleep(10);
             bool OFF = false;
             string frame1 = this.write(slaveId, startAddress3, func, OFF);//2049
             serialPort1.WriteLine(frame1);
             Thread.Sleep(10);
         }

         private void button7_Click(object sender, EventArgs e)// mo may 4
         {
             bool ON = true;
             string frame = this.write(slaveId, startAddress4, func, ON);
             serialPort1.WriteLine(frame);
             Thread.Sleep(10);
             bool OFF = false;
             string frame1 = this.write(slaveId, startAddress4, func, OFF);//2049
             serialPort1.WriteLine(frame1);
             Thread.Sleep(10);
         }
         public void button5Click(object sender, EventArgs e)// MO MAY 2
         {
             bool ON = true;
             string frame = this.write(slaveId, startAddress2, func, ON);
             serialPort1.WriteLine(frame);
             Thread.Sleep(10);
             bool OFF = false;
             string frame1 = this.write(slaveId, startAddress2, func, OFF);//2049
             serialPort1.WriteLine(frame1);
             Thread.Sleep(10);
         }

         public void button6Click(object sender, EventArgs e)// mo may 3
         {
             bool ON = true;
             string frame = this.write(slaveId, startAddress3, func, ON);
             serialPort1.WriteLine(frame);
             Thread.Sleep(10);
             bool OFF = false;
             string frame1 = this.write(slaveId, startAddress3, func, OFF);//2049
             serialPort1.WriteLine(frame1);
             Thread.Sleep(10);
         }

         public void button7Click(object sender, EventArgs e)// mo may 4
         {
             bool ON = true;
             string frame = this.write(slaveId, startAddress4, func, ON);
             serialPort1.WriteLine(frame);
             Thread.Sleep(10);
             bool OFF = false;
             string frame1 = this.write(slaveId, startAddress4, func, OFF);//2049
             serialPort1.WriteLine(frame1);
             Thread.Sleep(10);
         }

         private void label2_Click(object sender, EventArgs e)
         {

         }

         private void timer2_Tick(object sender, EventArgs e)
         {
             if (k1.Value == 0)
             {
                 if (mark1 == 1)
                 {
                     lam1.Text = "Get clothes";
                     //k1.PerformStep();
                     //k1.Value = 0;
                     l1.Text = "0";
                 }
                 else
                 {
                     lam1.Text = "Free";
                     //k1.PerformStep();
                     //k1.Value = 0;
                     l1.Text = "0";
                 }

             }
             else if (t1 == 0)// cua may say mo
             {
                 lam1.Text = "Waiting";// detect cua may say
                 nhan1 = 1;
             }
             else
             {
                 //lam1.Text = "running";
                 
                 if (dem1 == 60)
                 {
                     lam1.Text = "Running";
                     k1.PerformStep();
                     la1 = la1 - 1;
                     l1.Text = la1.ToString();
                     dem1 = 0;
                 }
                 else 
                 {
                     if (nhan1 == 1)
                     {
                         lam1.Text = "Running";
                         nhan1 = 0;
                     }
                     //lam1.Text = "running";
                     dem1 = dem1 + 1;
                     k1.PerformStep();
                 }

                 
             }

             //k1.Value = k1.Value - 1;
             //k1.PerformStep();
             
         }

         private void timer3_Tick(object sender, EventArgs e)
         {
             if (k2.Value == 0)
             {
                 if (mark2 == 1)
                 {
                     lam2.Text = "Get clothes";
                     //k1.PerformStep();
                     //k1.Value = 0;
                     l2.Text = "0";
                 }
                 else
                 {
                     lam2.Text = "Free";
                     //k1.PerformStep();
                     //k1.Value = 0;
                     l2.Text = "0";
                 }
             }
             else if (t2 == 0)
             {
                 lam2.Text = "Waiting";
                 nhan2 = 1;
             }
             else
             {
                 //lam2.Text = "running";
                 
                 if (dem2 == 60)
                 {
                     lam2.Text = "Running";
                     k2.PerformStep();
                     la2 = la2 - 1;
                     l2.Text = la2.ToString();
                     dem2 = 0;
                 }
                 else
                 {
                     if (nhan2 == 1)
                     {
                         lam2.Text = "Running";
                         nhan2 = 0;
                     }
                    
                     dem2 = dem2 + 1;
                     k2.PerformStep();
                 }
                 
             }

             //progressBar1.Value = progressBar1.Value - 1;
             //k2.PerformStep();
         }

         private void timer4_Tick(object sender, EventArgs e)
         {
             if (k3.Value == 0)
             {
                 if (mark3 == 1)
                 {
                     lam3.Text = "Get clothes";
                     //k1.PerformStep();
                     //k1.Value = 0;
                     l3.Text = "0";
                 }
                 else
                 {
                     lam3.Text = "Free";
                     //k1.PerformStep();
                     //k1.Value = 0;
                     l3.Text = "0";
                 }
             }
             else if (t3 == 0)
             {
                 lam3.Text = "Waiting";
                 nhan3=1;
             }
             else
             {
                 //lam3.Text = "running";
                 
                 if (dem3 == 60)
                 {
                     lam3.Text = "Running";
                     k3.PerformStep();
                     la3 = la3 - 1;
                     l3.Text = la3.ToString();
                     dem3 = 0;
                 }
                 else
                 {
                     if (nhan3 == 1) 
                     {
                         lam3.Text = "Running";
                         nhan3 = 0;
                     }
                    
                     dem3 = dem3 + 1;
                     k3.PerformStep();
                 }
                
             }

             //progressBar1.Value = progressBar1.Value - 1;
             //k3.PerformStep();
         }

         private void timer5_Tick(object sender, EventArgs e)
         {
             if (k4.Value == 0)
             {
                 if (mark4 == 1)
                 {
                     lam4.Text = "Get clothes";
                     //k1.PerformStep();
                     //k1.Value = 0;
                     l4.Text = "0";
                 }
                 else
                 {
                     lam4.Text = "Free";
                     //k1.PerformStep();
                     //k1.Value = 0;
                     l4.Text = "0";
                 }
             }
             else if (t4 == 0)
             {
                 lam4.Text = "Waiting";
                 nhan4 = 1;
             }
             else
             {
                 //lam4.Text = "running";

                 if (dem4 == 60)
                 {
                     lam4.Text = "Running";
                     k4.PerformStep();
                     la4 = la4 - 1;
                     l4.Text = la4.ToString();
                     dem4 = 0;
                 }
                 else 
                 {
                     if (nhan4 == 1)
                     {
                         lam4.Text = "Running";
                         nhan4 = 0;
                     }
                    
                     dem4 = dem4 + 1;
                     k4.PerformStep();
                 }
                 
             }

             //progressBar1.Value = progressBar1.Value - 1;
             //k4.PerformStep();
         }

         private void label4_Click(object sender, EventArgs e)
         {

         }

         private void button1_Click_1(object sender, EventArgs e)
         {
             int long3=0;
             long3 = Convert.ToInt32(textBox2.Text);
             if (long3 == 1)
             {
                 if (mark1 == 0)
                 {
                     label9.BackColor = Color.Red;
                     mark1 = 1;
                 }
                 else
                 {
                     mark1 = 0;
                     label9.BackColor = Color.Snow;
                 }
             }
             else if (long3 == 2)
             {
                 if (mark2 == 0)
                 {
                     label10.BackColor = Color.Red;
                     mark2 = 1;
                 }
                 else
                 {
                     mark2 = 0;
                     label10.BackColor = Color.Snow;
                 }
             }
             else if (long3 == 3)
             {
                 if (mark3 == 0)
                 {
                     label11.BackColor = Color.Red;
                     mark3 = 1;
                 }
                 else
                 {
                     mark3 = 0;
                     label11.BackColor = Color.Snow;
                 }
             }
             else if (long3 == 4)
             {
                 if (mark4 == 0)
                 {
                     label12.BackColor = Color.Red;
                     mark4 = 1;
                 }
                 else
                 {
                     mark4 = 0;
                     label12.BackColor = Color.Snow;
                 }
             }
             else
             {
                 MessageBox.Show("nhap sai so du vao");
             }

         }

         private void label17_Click(object sender, EventArgs e)
         {

         }

         private void timer6_Tick(object sender, EventArgs e)
         {
             if (k5.Value == 0)
             {
                 
                     lam5.Text = "Free";
                     //k1.PerformStep();
                     //k1.Value = 0;
                     l5.Text = "0";
                 

             }
             else if (t5 == 0)
             {
                 lam5.Text = "Waiting";
                 nhan5 = 1;
             }
             else
             {
                 //lam1.Text = "running";

                 if (dem5 == 60)
                 {
                     lam5.Text = "Running";
                     k5.PerformStep();
                     la5 = la5 - 1;
                     l5.Text = la5.ToString();
                     dem5 = 0;
                 }
                 else
                 {
                     if (nhan5 == 1)
                     {
                         lam5.Text = "Running";
                         nhan5 = 0;
                     }
                     //lam1.Text = "running";
                     dem5 = dem5 + 1;
                     k5.PerformStep();
                 }


             }

             //k1.Value = k1.Value - 1;
             //k1.PerformStep();
         }
        //KT
        
        }

      
        

       
        
    }

