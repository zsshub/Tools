namespace OsgConv
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class Form1 : Form
    {
        private Button button_Connection;
        private Button button1;
        private Button button2;
        private Button button3;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private CheckBox checkBox3;
        private CheckBox checkBox4;
        private CheckBox checkBox5;
        private ComboBox comboBox_Input;
        private ComboBox comboBox_Output;
        private IContainer components;
        private GroupBox groupBox_PagedLod;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label_classification_Col;
        private Label label_classification_Row;
        private Label label_LayerNum;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private TextBox textBox_ClaasificationRow;
        private TextBox textBox_ClassificationCol;
        private TextBox textBox_Input;
        private TextBox textBox_LevelNum;
        private TextBox textBox_OutPut;

        public Form1()
        {
            this.InitializeComponent();
            this.comboBox_Input.Items.Add(".osgb");
            this.comboBox_Input.Items.Add(".ive");
            this.comboBox_Input.Items.Add(".osg");
            this.comboBox_Input.Items.Add(".obj");
            this.comboBox_Input.SelectedIndex = -1;
            this.comboBox_Output.Items.Add(".osgb");
            this.comboBox_Output.Items.Add(".ive");
            this.comboBox_Output.Items.Add(".osg");
            this.comboBox_Output.Items.Add(".obj");
            this.comboBox_Output.SelectedIndex = -1;
        }

        private void button_Connection_Click(object sender, EventArgs e)
        {
            if (this.textBox_Input.Text == "")
            {
                MessageBox.Show("请选择输入路径");
            }
            else
            {
                string text = this.comboBox_Input.Text;
                string inputPath = this.textBox_Input.Text;
                this.setTrans(inputPath);
                if (this.radioButton1.Checked)
                {
                    if (text == ".osgb")
                    {
                        MessageBox.Show("osg2.8不支持osgb格式");
                        return;
                    }
                    osgConvCpp2_8.connectionOsgFile(inputPath, text);
                }
                else if (this.radioButton2.Checked)
                {
                    osgConvCpp3_2.connectionOsgFile(inputPath, text);
                }
                else if (this.radioButton3.Checked)
                {
                    osgConvCpp3_0.connectionOsgFile(inputPath, text);
                }
                MessageBox.Show("生成根结点成功");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((this.textBox_Input.Text == "") || (this.textBox_OutPut.Text == ""))
            {
                MessageBox.Show("请选择输入输出");
            }
            else
            {
                string text = this.comboBox_Input.Text;
                string outputType = this.comboBox_Output.Text;
                string inputPath = this.textBox_Input.Text;
                string str4 = this.textBox_OutPut.Text;
                bool bSaveImage = this.checkBox1.Checked;
                bool bSaveRoot = this.checkBox2.Checked;
                bool flag3 = false;
                if (bSaveRoot)
                {
                    this.setTrans(inputPath);
                }
                if (this.radioButton1.Checked)
                {
                    if (outputType == ".osgb")
                    {
                        MessageBox.Show("osg2.8不支持osgb格式！请重新选择");
                        return;
                    }
                    osgConvCpp2_8.convertOsgFile(inputPath + @"\", str4 + @"\", text, outputType, bSaveImage, bSaveRoot);
                    flag3 = true;
                }
                else if (this.radioButton2.Checked)
                {
                    if (this.groupBox_PagedLod.Visible)
                    {
                        int num;
                        int num2;
                        int num3;
                        int.TryParse(this.textBox_ClaasificationRow.Text, out num);
                        int.TryParse(this.textBox_ClassificationCol.Text, out num2);
                        int.TryParse(this.textBox_LevelNum.Text, out num3);
                        osgConvCpp3_2.setPagedLodPara(num, num2, num3);
                    }
                    flag3 = osgConvCpp3_2.convertOsgFile(inputPath + @"\", str4 + @"\", text, outputType, bSaveImage, bSaveRoot, this.checkBox3.Checked);
                }
                else if (this.radioButton3.Checked)
                {
                    if (this.groupBox_PagedLod.Visible)
                    {
                        int num4;
                        int num5;
                        int num6;
                        int.TryParse(this.textBox_ClaasificationRow.Text, out num4);
                        int.TryParse(this.textBox_ClassificationCol.Text, out num5);
                        int.TryParse(this.textBox_LevelNum.Text, out num6);
                        osgConvCpp3_0.setPagedLodPara(num4, num5, num6);
                    }
                    osgConvCpp3_0.setFlipZY(this.checkBox4.Checked);
                    bool bSmoothing = this.checkBox5.Checked && this.checkBox5.Visible;
                    flag3 = osgConvCpp3_0.convertOsgFile(inputPath + @"\", str4 + @"\", text, outputType, bSaveImage, bSaveRoot, this.checkBox3.Checked, bSmoothing);
                }
                if (flag3)
                {
                    MessageBox.Show("处理完成");
                }
                else
                {
                    MessageBox.Show("该数据是带调度的pagedLod数据，请不要选择生成PagedLod!");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox_Input.Text = dialog.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.textBox_OutPut.Text = dialog.SelectedPath;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBox3.Checked)
            {
                this.groupBox_PagedLod.Visible = true;
            }
            else
            {
                this.groupBox_PagedLod.Visible = false;
            }
        }

        private void comboBox_Output_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = this.comboBox_Output.Text;
            switch (text)
            {
                case ".osgb":
                case ".osg":
                    this.checkBox3.Visible = true;
                    if (this.checkBox3.Checked)
                    {
                        this.groupBox_PagedLod.Visible = true;
                    }
                    break;

                default:
                    this.checkBox3.Visible = false;
                    this.checkBox4.Visible = false;
                    break;
            }
            string str2 = this.comboBox_Input.Text;
            string str3 = this.comboBox_Output.Text;
            if (str2 == str3)
            {
                this.button_Connection.Visible = true;
                this.button1.Visible = false;
            }
            else
            {
                this.button_Connection.Visible = false;
                this.button1.Visible = true;
            }
            if ((text == ".osg") && this.radioButton3.Checked)
            {
                this.checkBox5.Visible = true;
            }
            else
            {
                this.checkBox5.Visible = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = this.comboBox_Input.Text;
            string str2 = this.comboBox_Output.Text;
            if (text == str2)
            {
                this.button_Connection.Visible = true;
                this.button1.Visible = false;
            }
            else
            {
                this.button_Connection.Visible = false;
                this.button1.Visible = true;
            }
            if (text == ".obj")
            {
                this.checkBox4.Visible = true;
            }
            else
            {
                this.checkBox4.Visible = false;
            }
            if ((text == ".obj") || (text == ".ive"))
            {
                this.radioButton1.Checked = true;
            }
            else
            {
                this.radioButton3.Enabled = true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void genPagedLod(string inputPath, string outputPath, string flip, string level)
        {
            int num8;
            string inputType = ".obj";
            string outputType = ".osgb";
            bool bSaveImage = true;
            bool bSaveRoot = true;
            if (bSaveRoot)
            {
                string path = inputPath + @"\report.txt";
                if (!File.Exists(path))
                {
                    osgConvCpp3_0.setTrans(0.0, 0.0, 0.0);
                }
                else
                {
                    string str4;
                    FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                    StreamReader reader = new StreamReader(stream);
                    while ((str4 = reader.ReadLine()) != null)
                    {
                        if (str4.IndexOf("X") != -1)
                        {
                            double num5;
                            double num6;
                            double num7;
                            int index = str4.IndexOf("Y");
                            int num2 = str4.IndexOf("X");
                            string s = str4.Substring(num2 + 3, index - 3);
                            int num3 = str4.IndexOf("Z") - index;
                            string str6 = str4.Substring(index + 3, num3 - 3);
                            int length = (str4.Length - str4.IndexOf("Z")) - 3;
                            string str7 = str4.Substring(str4.IndexOf("Z") + 3, length);
                            double.TryParse(s, out num5);
                            double.TryParse(str6, out num6);
                            double.TryParse(str7, out num7);
                            osgConvCpp3_0.setTrans(num5, num6, num7);
                            break;
                        }
                    }
                }
            }
            int.TryParse(level, out num8);
            osgConvCpp3_0.setPagedLodPara(4, 4, num8);
            if (flip == "y")
            {
                osgConvCpp3_0.setFlipZY(true);
            }
            else
            {
                osgConvCpp3_0.setFlipZY(false);
            }
            osgConvCpp3_0.convertOsgFile(inputPath + @"\", outputPath + @"\", inputType, outputType, bSaveImage, bSaveRoot, true, false);
        }

        private void getAllFilePostfixs(string inputPath, List<string> postFixList)
        {
            DirectoryInfo info = new DirectoryInfo(inputPath);
            foreach (DirectoryInfo info2 in info.GetDirectories())
            {
                string fullName = info2.FullName;
                this.getAllFilePostfixs(fullName, postFixList);
            }
            foreach (FileInfo info3 in info.GetFiles())
            {
                string name = info3.Name;
                int startIndex = name.LastIndexOf(".");
                int length = name.Length - startIndex;
                string item = name.Substring(startIndex, length);
                bool flag = false;
                foreach (string str4 in postFixList)
                {
                    if (str4 == item)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    postFixList.Add(item);
                }
            }
        }

        private void InitializeComponent()
        {
            ComponentResourceManager manager = new ComponentResourceManager(typeof(Form1));
            this.label1 = new Label();
            this.label2 = new Label();
            this.textBox_Input = new TextBox();
            this.textBox_OutPut = new TextBox();
            this.button1 = new Button();
            this.button2 = new Button();
            this.button3 = new Button();
            this.comboBox_Input = new ComboBox();
            this.label3 = new Label();
            this.checkBox1 = new CheckBox();
            this.radioButton1 = new RadioButton();
            this.radioButton2 = new RadioButton();
            this.checkBox2 = new CheckBox();
            this.checkBox3 = new CheckBox();
            this.groupBox_PagedLod = new GroupBox();
            this.textBox_LevelNum = new TextBox();
            this.label_LayerNum = new Label();
            this.label_classification_Col = new Label();
            this.label_classification_Row = new Label();
            this.textBox_ClassificationCol = new TextBox();
            this.textBox_ClaasificationRow = new TextBox();
            this.radioButton3 = new RadioButton();
            this.label4 = new Label();
            this.comboBox_Output = new ComboBox();
            this.groupBox1 = new GroupBox();
            this.groupBox2 = new GroupBox();
            this.checkBox5 = new CheckBox();
            this.checkBox4 = new CheckBox();
            this.button_Connection = new Button();
            this.label5 = new Label();
            this.groupBox_PagedLod.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x47, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "输入文件夹:";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(13, 50);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x47, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "输出文件夹:";
            this.textBox_Input.Location = new Point(90, 12);
            this.textBox_Input.Name = "textBox_Input";
            this.textBox_Input.Size = new Size(300, 0x15);
            this.textBox_Input.TabIndex = 2;
            this.textBox_Input.TextChanged += new EventHandler(this.textBox1_TextChanged);
            this.textBox_OutPut.Location = new Point(0x5b, 0x2f);
            this.textBox_OutPut.Name = "textBox_OutPut";
            this.textBox_OutPut.Size = new Size(300, 0x15);
            this.textBox_OutPut.TabIndex = 3;
            this.button1.Location = new Point(0x18c, 0x87);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x4b, 0x17);
            this.button1.TabIndex = 4;
            this.button1.Text = "开始转换";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.button2.Location = new Point(0x18c, 15);
            this.button2.Name = "button2";
            this.button2.Size = new Size(0x1c, 0x17);
            this.button2.TabIndex = 5;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.button2_Click);
            this.button3.Location = new Point(0x18c, 0x2c);
            this.button3.Name = "button3";
            this.button3.Size = new Size(0x1c, 0x17);
            this.button3.TabIndex = 6;
            this.button3.Text = "...";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new EventHandler(this.button3_Click);
            this.comboBox_Input.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox_Input.ForeColor = SystemColors.MenuText;
            this.comboBox_Input.FormattingEnabled = true;
            this.comboBox_Input.Location = new Point(0x4c, 0x53);
            this.comboBox_Input.Name = "comboBox_Input";
            this.comboBox_Input.Size = new Size(70, 20);
            this.comboBox_Input.TabIndex = 7;
            this.comboBox_Input.SelectedIndexChanged += new EventHandler(this.comboBox1_SelectedIndexChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(13, 0x58);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x3b, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "输入类型:";
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = CheckState.Checked;
            this.checkBox1.Location = new Point(10, 14);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(0x60, 0x10);
            this.checkBox1.TabIndex = 9;
            this.checkBox1.Text = "导出纹理图片";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new Point(0x1a, 15);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new Size(0x3b, 0x10);
            this.radioButton1.TabIndex = 10;
            this.radioButton1.Text = "osg2.8";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new Point(0x1a, 0x26);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new Size(0x3b, 0x10);
            this.radioButton2.TabIndex = 11;
            this.radioButton2.Text = "osg3.2";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.Visible = false;
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = CheckState.Checked;
            this.checkBox2.Location = new Point(10, 0x25);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new Size(0x54, 0x10);
            this.checkBox2.TabIndex = 12;
            this.checkBox2.Text = "生成根文件";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new Point(10, 0x3b);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new Size(0x60, 0x10);
            this.checkBox3.TabIndex = 13;
            this.checkBox3.Text = "生成PagedLod";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.Visible = false;
            this.checkBox3.CheckedChanged += new EventHandler(this.checkBox3_CheckedChanged);
            this.groupBox_PagedLod.Controls.Add(this.textBox_LevelNum);
            this.groupBox_PagedLod.Controls.Add(this.label_LayerNum);
            this.groupBox_PagedLod.Controls.Add(this.label_classification_Col);
            this.groupBox_PagedLod.Controls.Add(this.label_classification_Row);
            this.groupBox_PagedLod.Controls.Add(this.textBox_ClassificationCol);
            this.groupBox_PagedLod.Controls.Add(this.textBox_ClaasificationRow);
            this.groupBox_PagedLod.Location = new Point(0x1d, 0xd7);
            this.groupBox_PagedLod.Name = "groupBox_PagedLod";
            this.groupBox_PagedLod.Size = new Size(0x16a, 0x2f);
            this.groupBox_PagedLod.TabIndex = 14;
            this.groupBox_PagedLod.TabStop = false;
            this.groupBox_PagedLod.Text = "PagedLod参数设置";
            this.groupBox_PagedLod.Visible = false;
            this.textBox_LevelNum.Location = new Point(0xd7, 15);
            this.textBox_LevelNum.Name = "textBox_LevelNum";
            this.textBox_LevelNum.Size = new Size(0x25, 0x15);
            this.textBox_LevelNum.TabIndex = 0x10;
            this.textBox_LevelNum.Text = "3";
            this.label_LayerNum.AutoSize = true;
            this.label_LayerNum.Location = new Point(0xa8, 0x15);
            this.label_LayerNum.Name = "label_LayerNum";
            this.label_LayerNum.Size = new Size(0x29, 12);
            this.label_LayerNum.TabIndex = 15;
            this.label_LayerNum.Text = "层数：";
            this.label_classification_Col.AutoSize = true;
            this.label_classification_Col.Location = new Point(0x61, 20);
            this.label_classification_Col.Name = "label_classification_Col";
            this.label_classification_Col.Size = new Size(0x1d, 12);
            this.label_classification_Col.TabIndex = 14;
            this.label_classification_Col.Text = "列：";
            this.label_classification_Row.AutoSize = true;
            this.label_classification_Row.Location = new Point(0x1a, 0x15);
            this.label_classification_Row.Name = "label_classification_Row";
            this.label_classification_Row.Size = new Size(0x1d, 12);
            this.label_classification_Row.TabIndex = 13;
            this.label_classification_Row.Text = "行：";
            this.textBox_ClassificationCol.Location = new Point(0x84, 15);
            this.textBox_ClassificationCol.Name = "textBox_ClassificationCol";
            this.textBox_ClassificationCol.Size = new Size(30, 0x15);
            this.textBox_ClassificationCol.TabIndex = 12;
            this.textBox_ClassificationCol.Text = "4";
            this.textBox_ClaasificationRow.Location = new Point(0x3d, 0x12);
            this.textBox_ClaasificationRow.Name = "textBox_ClaasificationRow";
            this.textBox_ClaasificationRow.Size = new Size(30, 0x15);
            this.textBox_ClaasificationRow.TabIndex = 11;
            this.textBox_ClaasificationRow.Text = "4";
            this.radioButton3.AutoSize = true;
            this.radioButton3.Checked = true;
            this.radioButton3.Location = new Point(0x1a, 0x3b);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new Size(0x3b, 0x10);
            this.radioButton3.TabIndex = 15;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "osg3.0";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new EventHandler(this.radioButton3_CheckedChanged);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(13, 120);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x3b, 12);
            this.label4.TabIndex = 0x11;
            this.label4.Text = "输出类型:";
            this.comboBox_Output.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox_Output.ForeColor = SystemColors.MenuText;
            this.comboBox_Output.FormattingEnabled = true;
            this.comboBox_Output.Location = new Point(0x4c, 0x73);
            this.comboBox_Output.Name = "comboBox_Output";
            this.comboBox_Output.Size = new Size(70, 20);
            this.comboBox_Output.TabIndex = 0x10;
            this.comboBox_Output.SelectedIndexChanged += new EventHandler(this.comboBox_Output_SelectedIndexChanged);
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new Point(0x11e, 0x53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0x68, 0x5f);
            this.groupBox1.TabIndex = 0x12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "版本：";
            this.groupBox2.Controls.Add(this.checkBox5);
            this.groupBox2.Controls.Add(this.checkBox4);
            this.groupBox2.Controls.Add(this.checkBox3);
            this.groupBox2.Controls.Add(this.checkBox2);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Location = new Point(160, 0x53);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0x75, 0x7e);
            this.groupBox2.TabIndex = 0x13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "操作:";
            this.checkBox5.AutoSize = true;
            this.checkBox5.Checked = true;
            this.checkBox5.CheckState = CheckState.Checked;
            this.checkBox5.Location = new Point(10, 0x51);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new Size(0x48, 0x10);
            this.checkBox5.TabIndex = 14;
            this.checkBox5.Text = "自动法线";
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.Visible = false;
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new Point(10, 0x67);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new Size(0x4e, 0x10);
            this.checkBox4.TabIndex = 14;
            this.checkBox4.Text = "翻转z-y轴";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.Visible = false;
            this.button_Connection.Location = new Point(0x18c, 0x6a);
            this.button_Connection.Name = "button_Connection";
            this.button_Connection.Size = new Size(0x4b, 0x17);
            this.button_Connection.TabIndex = 20;
            this.button_Connection.Text = "生成根结点";
            this.button_Connection.UseVisualStyleBackColor = true;
            this.button_Connection.Visible = false;
            this.button_Connection.Click += new EventHandler(this.button_Connection_Click);
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0x1b3, 0x115);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x41, 12);
            this.label5.TabIndex = 0x15;
            this.label5.Text = "版本v1.1.1";
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x1f5, 0x13a);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.button_Connection);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.comboBox_Output);
            base.Controls.Add(this.groupBox_PagedLod);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.comboBox_Input);
            base.Controls.Add(this.button3);
            base.Controls.Add(this.button2);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.textBox_OutPut);
            base.Controls.Add(this.textBox_Input);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            //base.Icon = (Icon) manager.GetObject("$this.Icon");
            base.MaximizeBox = false;
            base.Name = "Form1";
            this.Text = "OSG格式转换工具";
            this.groupBox_PagedLod.ResumeLayout(false);
            this.groupBox_PagedLod.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.checkBox5.Visible = this.radioButton3.Checked;
        }

        private void setTrans(string inputPath)
        {
            string path = inputPath + @"\report.txt";
            if (!File.Exists(path))
            {
                MessageBox.Show("没有report.txt文件，将会为您设置偏移量为0");
                if (this.radioButton1.Checked)
                {
                    osgConvCpp2_8.setTrans(0.0, 0.0, 0.0);
                }
                else if (this.radioButton2.Checked)
                {
                    osgConvCpp3_2.setTrans(0.0, 0.0, 0.0);
                }
                else if (this.radioButton3.Checked)
                {
                    osgConvCpp3_0.setTrans(0.0, 0.0, 0.0);
                }
            }
            else
            {
                string str2;
                FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(stream);
                while ((str2 = reader.ReadLine()) != null)
                {
                    if (str2.IndexOf("X") != -1)
                    {
                        double num5;
                        double num6;
                        double num7;
                        int index = str2.IndexOf("Y");
                        int num2 = str2.IndexOf("X");
                        string s = str2.Substring(num2 + 3, index - 3);
                        int num3 = str2.IndexOf("Z") - index;
                        string str4 = str2.Substring(index + 3, num3 - 3);
                        int length = (str2.Length - str2.IndexOf("Z")) - 3;
                        string str5 = str2.Substring(str2.IndexOf("Z") + 3, length);
                        double.TryParse(s, out num5);
                        double.TryParse(str4, out num6);
                        double.TryParse(str5, out num7);
                        if (this.radioButton1.Checked)
                        {
                            osgConvCpp2_8.setTrans(num5, num6, num7);
                            return;
                        }
                        if (this.radioButton2.Checked)
                        {
                            osgConvCpp3_2.setTrans(num5, num6, num7);
                            return;
                        }
                        if (!this.radioButton3.Checked)
                        {
                            break;
                        }
                        osgConvCpp3_0.setTrans(num5, num6, num7);
                        return;
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string text = this.textBox_Input.Text;
            List<string> postFixList = new List<string>();
            this.getAllFilePostfixs(text, postFixList);
        }
    }
}

