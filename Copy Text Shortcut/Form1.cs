using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Threading;
using Copy_Text_Shortcut.Model;
using System.Diagnostics;

namespace Copy_Text_Shortcut
{
    public partial class Form1 : Form
    {
        bool isRunning;
        int currentIndex = 0;
        int locationAxisY = 20;
        int[] decimalKeyValue;
        List<Decimal_TextBoxMap> lstItem;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            isRunning = true;
            decimalKeyValue = new int[] { 35, 36, 37, 38, 39, 40, 41, 42, 43 };

            lstItem = new List<Decimal_TextBoxMap>
            {
                new Decimal_TextBoxMap { DecimalKeyValue = 35, TxtboxObj = txtShortcut1 }
            };

            Thread Th = new Thread(KeyboardKeyEvent);
            Th.SetApartmentState(ApartmentState.STA);
            CheckForIllegalCrossThreadCalls = false;
            Th.Start();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            isRunning = false;
        }

        void KeyboardKeyEvent()
        {
            while (isRunning)
            {
                Thread.Sleep(50);

                foreach (var item in lstItem)
                {
                    //Shortcut key are assigned for copying & pasting a value, these are Key press Event
                    if ((Keyboard.GetKeyStates(Key.C) & Keyboard.GetKeyStates((Key)item.DecimalKeyValue) & KeyStates.Down) > 0)
                        CopyAndPaste(item.TxtboxObj);
                }
            }
        }

        void CopyAndPaste(TextBox TxtBoxObj)
        {
            //Copying a value in Clipboard from textbox
            Clipboard.SetText(String.IsNullOrWhiteSpace(TxtBoxObj.Text)? "[No Text]": TxtBoxObj.Text);

            //Sending backspacekey twice & Control + V command to paste the value on the current application
            SendKeys.SendWait("{BACKSPACE 2}^v");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (currentIndex <= 7)
            {
                locationAxisY += 40;
                currentIndex++;

                var dynLabel = new Label { Text = "Shortcut C + " + (currentIndex + 1), Location = new Point(13, locationAxisY + 5) };
                dynLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dynLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
                dynLabel.Size = new System.Drawing.Size(102, 17);

                this.Controls.Add(dynLabel);

                var dynTxtBox = new TextBox { Name = "txtShortcut" + currentIndex + 1, Location = new Point(128, locationAxisY) };
                dynTxtBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dynTxtBox.Margin = new System.Windows.Forms.Padding(4);
                dynTxtBox.Size = new Size(363, 30);
                this.Controls.Add(dynTxtBox);

                lstItem.Add(new Decimal_TextBoxMap { DecimalKeyValue = decimalKeyValue[currentIndex], TxtboxObj = dynTxtBox });
            }
            else
            {
                MessageBox.Show("Cannot add more item, please contact to developer Sourabh Chawla, request on email id: chawlasourabh01@gmail.com", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }    
}
