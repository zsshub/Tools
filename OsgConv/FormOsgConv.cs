namespace OsgConv
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class FormOsgConv : Form
    {
        string tempObjDir = Application.StartupPath + "\\TempObjDir";
        string obj2gltf = Application.StartupPath + "\\obj2gltf.bat";

        private Button createRootBtn;
        private Button btnTrans;
        private Button btnInput;
        private Button btnOutput;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private CheckBox checkBox3;
        private CheckBox checkBox4;
        private CheckBox checkBox5;
        private ComboBox cmbInputType;
        private ComboBox cmbOutputType;
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
        private RadioButton radioBtn_osg2_8;
        private RadioButton radioBtn_osg3_2;
        private RadioButton radioBtn_osg3_0;
        private TextBox textBox_ClaasificationRow;
        private TextBox textBox_ClassificationCol;
        private TextBox txtInput;
        private TextBox textBox_LevelNum;
        private TextBox txtOutput;

        public FormOsgConv()
        {
            this.InitializeComponent();
            this.cmbInputType.Items.Add(".osgb");
            this.cmbInputType.Items.Add(".ive");
            this.cmbInputType.Items.Add(".osg");
            this.cmbInputType.Items.Add(".obj");
            this.cmbInputType.SelectedIndex = 0;
            this.cmbOutputType.Items.Add(".osgb");
            this.cmbOutputType.Items.Add(".ive");
            this.cmbOutputType.Items.Add(".osg");
            this.cmbOutputType.Items.Add(".obj");
            this.cmbOutputType.Items.Add(".gltf");
            this.cmbOutputType.SelectedIndex = 3;
            txtInput.Text = @"C:\Users\SAI\Desktop\osg";
            //txtOutput.Text = @"C:\Users\SAI\Desktop\新建 文件夹";
        }

        private void button_Connection_Click(object sender, EventArgs e)
        {
            if (this.txtInput.Text == "")
            {
                MessageBox.Show("请选择输入路径");
            }
            else
            {
                string text = this.cmbInputType.Text;
                string inputPath = this.txtInput.Text;
                this.setTrans(inputPath);
                if (this.radioBtn_osg2_8.Checked)
                {
                    if (text == ".osgb")
                    {
                        MessageBox.Show("osg2.8不支持osgb格式");
                        return;
                    }
                    osgConvCpp2_8.connectionOsgFile(inputPath, text);
                }
                else if (this.radioBtn_osg3_2.Checked)
                {
                    osgConvCpp3_2.connectionOsgFile(inputPath, text);
                }
                else if (this.radioBtn_osg3_0.Checked)
                {
                    osgConvCpp3_0.connectionOsgFile(inputPath, text);
                }
                MessageBox.Show("生成根结点成功");
            }
        }

        private void btnTrans_Click(object sender, EventArgs e)
        {

            //清除obj缓存
            if (Directory.Exists(tempObjDir))
                Directory.Delete(tempObjDir, true);
            Directory.CreateDirectory(tempObjDir);

            if (!Directory.Exists(this.txtInput.Text) || !Directory.Exists(this.txtOutput.Text))
            {
                MessageBox.Show("请选择输入输出");
            }

            string inputPath = this.txtInput.Text;
            string outputPath = this.txtOutput.Text;
            string inputType = this.cmbInputType.Text;
            string outputType = this.cmbOutputType.Text;

            bool bSaveImage = this.checkBox1.Checked;
            bool bSaveRoot = this.checkBox2.Checked;

            string osgVersion = "";
            if (this.radioBtn_osg2_8.Checked)
                osgVersion = "osg2_8";
            else if (this.radioBtn_osg3_0.Checked)
                osgVersion = "osg3_0";
            else if (this.radioBtn_osg3_2.Checked)
                osgVersion = "osg3_2";

            this.Text = "格式转换中……";
            ConvertOsgFile(osgVersion, inputPath, outputPath, inputType, outputType, bSaveImage, bSaveRoot);
            this.Text = "格式转换完成";
        }
        void ConvertOsgFile(string osgVersion, string inputPath, string outputPath, string inputType, string outputType, bool bSaveImage, bool bSaveRoot)
        {
            if (outputType == ".gltf")
            {
                outputPath = tempObjDir;
                outputType = ".obj";
            }

            bool isOk = false;
            if (bSaveRoot)
                this.setTrans(inputPath);

            if (osgVersion == "osg2_8")
            {
                if (outputType == ".osgb")
                {
                    MessageBox.Show("osg2.8不支持osgb格式！请重新选择");
                    return;
                }
                osgConvCpp2_8.convertOsgFile(inputPath + @"\", outputPath + @"\", inputType, outputType, bSaveImage, bSaveRoot);
                isOk = true;
            }
            //默认使用osgConvCpp3_0转换
            else if (osgVersion == "osg3_0")
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

                isOk = osgConvCpp3_0.convertOsgFile(inputPath + @"\", outputPath + @"\", inputType, outputType, bSaveImage, bSaveRoot, this.checkBox3.Checked, bSmoothing);
            }
            else if (osgVersion == "osg3_2")
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
                isOk = osgConvCpp3_2.convertOsgFile(inputPath + @"\", outputPath + @"\", inputType, outputType, bSaveImage, bSaveRoot, this.checkBox3.Checked);
            }

            if (isOk)
            {
                if (this.cmbOutputType.Text == ".gltf")
                    Convert_Objs_To_Gltfs(tempObjDir, this.txtOutput.Text);

                MessageBox.Show("处理完成！", "提示");
            }
            else
                MessageBox.Show("该数据是带调度的pagedLod数据，请不要选择生成PagedLod!");

        }

        /// <summary>
        /// objs转gltfs
        /// </summary>
        /// <param name="objsDir"></param>
        void Convert_Objs_To_Gltfs(string objsDir, string gltfsDir)
        {
            if (!Directory.Exists(objsDir) || !Directory.Exists(gltfsDir)) return;

            ProcessChineseChar(tempObjDir);

            if (System.IO.File.Exists(obj2gltf))
            {
                ModifyBatFile(obj2gltf, objsDir, gltfsDir);

                using (Process myPro = new Process())
                {
                    myPro.StartInfo.FileName = obj2gltf;
                    myPro.StartInfo.UseShellExecute = true;//设置为fasle,无结果输出
                    myPro.StartInfo.CreateNoWindow = true;
                    myPro.StartInfo.WorkingDirectory = Application.StartupPath;//设置当前工作目录
                    myPro.StartInfo.Verb = "runas";//管理员运行
                    myPro.Start();
                    myPro.WaitForExit();
                }
            }
        }

        /// <summary>
        /// 修改批处理文件输入输出路径
        /// </summary>
        /// <param name="obj2gltfPath"></param>
        /// <param name="inputPath"></param>
        /// <param name="outputPath"></param>
        void ModifyBatFile(string obj2gltfPath, string inputPath, string outputPath)
        {
            string[] strArr = System.IO.File.ReadAllLines(obj2gltfPath, System.Text.Encoding.Default);

            string inPath = null;
            string outPath = null;
            foreach (var str in strArr)
            {
                int inIdx = str.IndexOf("input=");
                if (inIdx != -1)
                    inPath = str.Substring(inIdx);
                int outIdx = str.IndexOf("output=");
                if (outIdx != -1)
                    outPath = str.Substring(outIdx);
            }
            if (inPath != null && outPath != null)
            {
                string content = System.IO.File.ReadAllText(obj2gltfPath, System.Text.Encoding.Default);
                content = content.Replace(inPath, "input=" + inputPath);
                content = content.Replace(outPath, "output=" + outputPath);
                System.IO.File.WriteAllText(obj2gltfPath, content, System.Text.Encoding.Default);
            }
        }

        /// <summary>
        /// 处理obj模型中文字符问题
        /// </summary>
        /// <param name="objDir"></param>
        void ProcessChineseChar(string objDir)
        {
            string[] mtls = System.IO.Directory.GetFiles(objDir, "*.mtl");
            for (int i = 0; i < mtls?.Length; i++)
            {
                string content = System.IO.File.ReadAllText(mtls[i], System.Text.Encoding.Default);
                if (content.IndexOf("map_Kd") != -1)
                {
                    string imgPath = content.Substring(content.LastIndexOf("map_Kd") + 6).Trim();
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(imgPath);

                    if (IsChineseChar(fileName))
                    {
                        //图片名称为中文的替换为guid
                        string guid = Guid.NewGuid().ToString();
                        System.IO.File.WriteAllText(mtls[i], content.Replace(fileName, guid));

                        imgPath = System.IO.Path.GetDirectoryName(mtls[i]) + "\\" + imgPath;
                        string newImgPath = imgPath.Replace(fileName, guid);
                        if (System.IO.File.Exists(imgPath))
                        {
                            System.IO.FileInfo imgInfo = new System.IO.FileInfo(imgPath);
                            imgInfo.MoveTo(newImgPath);
                        }
                    }
                }

            }

        }


        /// <summary>
        /// 判断字符串是否为汉字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        bool IsChineseChar(string str)
        {
            if (str != null)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if ((int)str[i] > 127)
                        return true;
                }
            }
            return false;
        }


        private void btnInput_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.RootFolder = Environment.SpecialFolder.DesktopDirectory;
            if (dialog.ShowDialog() == DialogResult.OK)
                this.txtInput.Text = dialog.SelectedPath;
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
                this.txtOutput.Text = dialog.SelectedPath;
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

        private void cmbOutputType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = this.cmbOutputType.Text;
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
            string str2 = this.cmbInputType.Text;
            string str3 = this.cmbOutputType.Text;
            if (str2 == str3)
            {
                this.createRootBtn.Visible = true;
                this.btnTrans.Visible = false;
            }
            else
            {
                this.createRootBtn.Visible = false;
                this.btnTrans.Visible = true;
            }
            if ((text == ".osg") && this.radioBtn_osg3_0.Checked)
            {
                this.checkBox5.Visible = true;
            }
            else
            {
                this.checkBox5.Visible = false;
            }
        }

        private void cmbInputType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string inputType = this.cmbInputType.Text;
            string outputType = this.cmbOutputType.Text;

            if (inputType == outputType)
            {
                this.createRootBtn.Visible = true;
                this.btnTrans.Visible = false;
            }
            else
            {
                this.createRootBtn.Visible = false;
                this.btnTrans.Visible = true;
            }
            if (inputType == ".obj")
            {
                this.checkBox4.Visible = true;
            }
            else
            {
                this.checkBox4.Visible = false;
            }
            if ((inputType == ".obj") || (inputType == ".ive"))
            {
                this.radioBtn_osg2_8.Checked = true;
            }
            else
            {
                this.radioBtn_osg3_0.Enabled = true;
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnTrans = new System.Windows.Forms.Button();
            this.btnInput = new System.Windows.Forms.Button();
            this.btnOutput = new System.Windows.Forms.Button();
            this.cmbInputType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.radioBtn_osg2_8 = new System.Windows.Forms.RadioButton();
            this.radioBtn_osg3_2 = new System.Windows.Forms.RadioButton();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.groupBox_PagedLod = new System.Windows.Forms.GroupBox();
            this.textBox_LevelNum = new System.Windows.Forms.TextBox();
            this.label_LayerNum = new System.Windows.Forms.Label();
            this.label_classification_Col = new System.Windows.Forms.Label();
            this.label_classification_Row = new System.Windows.Forms.Label();
            this.textBox_ClassificationCol = new System.Windows.Forms.TextBox();
            this.textBox_ClaasificationRow = new System.Windows.Forms.TextBox();
            this.radioBtn_osg3_0 = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbOutputType = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.createRootBtn = new System.Windows.Forms.Button();
            this.groupBox_PagedLod.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "输入文件夹:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "输出文件夹:";
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(90, 12);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(300, 21);
            this.txtInput.TabIndex = 2;
            this.txtInput.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(90, 47);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(301, 21);
            this.txtOutput.TabIndex = 3;
            // 
            // btnTrans
            // 
            this.btnTrans.Location = new System.Drawing.Point(312, 184);
            this.btnTrans.Name = "btnTrans";
            this.btnTrans.Size = new System.Drawing.Size(82, 23);
            this.btnTrans.TabIndex = 4;
            this.btnTrans.Text = "开始转换";
            this.btnTrans.UseVisualStyleBackColor = true;
            this.btnTrans.Click += new System.EventHandler(this.btnTrans_Click);
            // 
            // btnInput
            // 
            this.btnInput.Location = new System.Drawing.Point(396, 10);
            this.btnInput.Name = "btnInput";
            this.btnInput.Size = new System.Drawing.Size(28, 23);
            this.btnInput.TabIndex = 5;
            this.btnInput.Text = "...";
            this.btnInput.UseVisualStyleBackColor = true;
            this.btnInput.Click += new System.EventHandler(this.btnInput_Click);
            // 
            // btnOutput
            // 
            this.btnOutput.Location = new System.Drawing.Point(397, 45);
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(28, 23);
            this.btnOutput.TabIndex = 6;
            this.btnOutput.Text = "...";
            this.btnOutput.UseVisualStyleBackColor = true;
            this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);
            // 
            // cmbInputType
            // 
            this.cmbInputType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInputType.ForeColor = System.Drawing.SystemColors.MenuText;
            this.cmbInputType.FormattingEnabled = true;
            this.cmbInputType.Location = new System.Drawing.Point(76, 88);
            this.cmbInputType.Name = "cmbInputType";
            this.cmbInputType.Size = new System.Drawing.Size(70, 20);
            this.cmbInputType.TabIndex = 7;
            this.cmbInputType.SelectedIndexChanged += new System.EventHandler(this.cmbInputType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "输入类型:";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(10, 14);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(96, 16);
            this.checkBox1.TabIndex = 9;
            this.checkBox1.Text = "导出纹理图片";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // radioBtn_osg2_8
            // 
            this.radioBtn_osg2_8.AutoSize = true;
            this.radioBtn_osg2_8.Location = new System.Drawing.Point(26, 15);
            this.radioBtn_osg2_8.Name = "radioBtn_osg2_8";
            this.radioBtn_osg2_8.Size = new System.Drawing.Size(59, 16);
            this.radioBtn_osg2_8.TabIndex = 10;
            this.radioBtn_osg2_8.Text = "osg2.8";
            this.radioBtn_osg2_8.UseVisualStyleBackColor = true;
            // 
            // radioBtn_osg3_2
            // 
            this.radioBtn_osg3_2.AutoSize = true;
            this.radioBtn_osg3_2.Checked = true;
            this.radioBtn_osg3_2.Location = new System.Drawing.Point(26, 59);
            this.radioBtn_osg3_2.Name = "radioBtn_osg3_2";
            this.radioBtn_osg3_2.Size = new System.Drawing.Size(59, 16);
            this.radioBtn_osg3_2.TabIndex = 11;
            this.radioBtn_osg3_2.TabStop = true;
            this.radioBtn_osg3_2.Text = "osg3.2";
            this.radioBtn_osg3_2.UseVisualStyleBackColor = true;
            this.radioBtn_osg3_2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(10, 37);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(84, 16);
            this.checkBox2.TabIndex = 12;
            this.checkBox2.Text = "生成根文件";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(10, 59);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(96, 16);
            this.checkBox3.TabIndex = 13;
            this.checkBox3.Text = "生成PagedLod";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.Visible = false;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // groupBox_PagedLod
            // 
            this.groupBox_PagedLod.Controls.Add(this.textBox_LevelNum);
            this.groupBox_PagedLod.Controls.Add(this.label_LayerNum);
            this.groupBox_PagedLod.Controls.Add(this.label_classification_Col);
            this.groupBox_PagedLod.Controls.Add(this.label_classification_Row);
            this.groupBox_PagedLod.Controls.Add(this.textBox_ClassificationCol);
            this.groupBox_PagedLod.Controls.Add(this.textBox_ClaasificationRow);
            this.groupBox_PagedLod.Location = new System.Drawing.Point(22, 401);
            this.groupBox_PagedLod.Name = "groupBox_PagedLod";
            this.groupBox_PagedLod.Size = new System.Drawing.Size(362, 47);
            this.groupBox_PagedLod.TabIndex = 14;
            this.groupBox_PagedLod.TabStop = false;
            this.groupBox_PagedLod.Text = "PagedLod参数设置";
            this.groupBox_PagedLod.Visible = false;
            // 
            // textBox_LevelNum
            // 
            this.textBox_LevelNum.Location = new System.Drawing.Point(215, 15);
            this.textBox_LevelNum.Name = "textBox_LevelNum";
            this.textBox_LevelNum.Size = new System.Drawing.Size(37, 21);
            this.textBox_LevelNum.TabIndex = 16;
            this.textBox_LevelNum.Text = "3";
            // 
            // label_LayerNum
            // 
            this.label_LayerNum.AutoSize = true;
            this.label_LayerNum.Location = new System.Drawing.Point(168, 21);
            this.label_LayerNum.Name = "label_LayerNum";
            this.label_LayerNum.Size = new System.Drawing.Size(41, 12);
            this.label_LayerNum.TabIndex = 15;
            this.label_LayerNum.Text = "层数：";
            // 
            // label_classification_Col
            // 
            this.label_classification_Col.AutoSize = true;
            this.label_classification_Col.Location = new System.Drawing.Point(97, 20);
            this.label_classification_Col.Name = "label_classification_Col";
            this.label_classification_Col.Size = new System.Drawing.Size(29, 12);
            this.label_classification_Col.TabIndex = 14;
            this.label_classification_Col.Text = "列：";
            // 
            // label_classification_Row
            // 
            this.label_classification_Row.AutoSize = true;
            this.label_classification_Row.Location = new System.Drawing.Point(26, 21);
            this.label_classification_Row.Name = "label_classification_Row";
            this.label_classification_Row.Size = new System.Drawing.Size(29, 12);
            this.label_classification_Row.TabIndex = 13;
            this.label_classification_Row.Text = "行：";
            // 
            // textBox_ClassificationCol
            // 
            this.textBox_ClassificationCol.Location = new System.Drawing.Point(132, 15);
            this.textBox_ClassificationCol.Name = "textBox_ClassificationCol";
            this.textBox_ClassificationCol.Size = new System.Drawing.Size(30, 21);
            this.textBox_ClassificationCol.TabIndex = 12;
            this.textBox_ClassificationCol.Text = "4";
            // 
            // textBox_ClaasificationRow
            // 
            this.textBox_ClaasificationRow.Location = new System.Drawing.Point(61, 18);
            this.textBox_ClaasificationRow.Name = "textBox_ClaasificationRow";
            this.textBox_ClaasificationRow.Size = new System.Drawing.Size(30, 21);
            this.textBox_ClaasificationRow.TabIndex = 11;
            this.textBox_ClaasificationRow.Text = "4";
            // 
            // radioBtn_osg3_0
            // 
            this.radioBtn_osg3_0.AutoSize = true;
            this.radioBtn_osg3_0.Location = new System.Drawing.Point(26, 36);
            this.radioBtn_osg3_0.Name = "radioBtn_osg3_0";
            this.radioBtn_osg3_0.Size = new System.Drawing.Size(59, 16);
            this.radioBtn_osg3_0.TabIndex = 15;
            this.radioBtn_osg3_0.Text = "osg3.0";
            this.radioBtn_osg3_0.UseVisualStyleBackColor = true;
            this.radioBtn_osg3_0.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 17;
            this.label4.Text = "输出类型:";
            // 
            // cmbOutputType
            // 
            this.cmbOutputType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOutputType.ForeColor = System.Drawing.SystemColors.MenuText;
            this.cmbOutputType.FormattingEnabled = true;
            this.cmbOutputType.Location = new System.Drawing.Point(76, 129);
            this.cmbOutputType.Name = "cmbOutputType";
            this.cmbOutputType.Size = new System.Drawing.Size(70, 20);
            this.cmbOutputType.TabIndex = 16;
            this.cmbOutputType.SelectedIndexChanged += new System.EventHandler(this.cmbOutputType_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioBtn_osg3_0);
            this.groupBox1.Controls.Add(this.radioBtn_osg3_2);
            this.groupBox1.Controls.Add(this.radioBtn_osg2_8);
            this.groupBox1.Location = new System.Drawing.Point(309, 81);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(104, 95);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "版本：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox5);
            this.groupBox2.Controls.Add(this.checkBox4);
            this.groupBox2.Controls.Add(this.checkBox3);
            this.groupBox2.Controls.Add(this.checkBox2);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Location = new System.Drawing.Point(170, 81);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(117, 126);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "操作:";
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Checked = true;
            this.checkBox5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox5.Location = new System.Drawing.Point(10, 81);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(72, 16);
            this.checkBox5.TabIndex = 14;
            this.checkBox5.Text = "自动法线";
            this.checkBox5.UseVisualStyleBackColor = true;
            this.checkBox5.Visible = false;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(10, 103);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(78, 16);
            this.checkBox4.TabIndex = 14;
            this.checkBox4.Text = "翻转z-y轴";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.Visible = false;
            // 
            // createRootBtn
            // 
            this.createRootBtn.Location = new System.Drawing.Point(312, 333);
            this.createRootBtn.Name = "createRootBtn";
            this.createRootBtn.Size = new System.Drawing.Size(101, 23);
            this.createRootBtn.TabIndex = 20;
            this.createRootBtn.Text = "生成根结点";
            this.createRootBtn.UseVisualStyleBackColor = true;
            this.createRootBtn.Visible = false;
            this.createRootBtn.Click += new System.EventHandler(this.button_Connection_Click);
            // 
            // FormOsg2gltf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 227);
            this.Controls.Add(this.createRootBtn);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbOutputType);
            this.Controls.Add(this.groupBox_PagedLod);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbInputType);
            this.Controls.Add(this.btnOutput);
            this.Controls.Add(this.btnInput);
            this.Controls.Add(this.btnTrans);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormOsg2gltf";
            this.Text = "格式转换工具";
            this.groupBox_PagedLod.ResumeLayout(false);
            this.groupBox_PagedLod.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.checkBox5.Visible = this.radioBtn_osg3_0.Checked;
        }

        private void setTrans(string inputPath)
        {
            string path = inputPath + @"\report.txt";
            if (!File.Exists(path))
            {
                MessageBox.Show("没有report.txt文件，将会为您设置偏移量为0");
                if (this.radioBtn_osg2_8.Checked)
                {
                    osgConvCpp2_8.setTrans(0.0, 0.0, 0.0);
                }
                else if (this.radioBtn_osg3_2.Checked)
                {
                    osgConvCpp3_2.setTrans(0.0, 0.0, 0.0);
                }
                else if (this.radioBtn_osg3_0.Checked)
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
                        if (this.radioBtn_osg2_8.Checked)
                        {
                            osgConvCpp2_8.setTrans(num5, num6, num7);
                            return;
                        }
                        if (this.radioBtn_osg3_2.Checked)
                        {
                            osgConvCpp3_2.setTrans(num5, num6, num7);
                            return;
                        }
                        if (!this.radioBtn_osg3_0.Checked)
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
            string text = this.txtInput.Text;
            List<string> postFixList = new List<string>();
            this.getAllFilePostfixs(text, postFixList);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}

