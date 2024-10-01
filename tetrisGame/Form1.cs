using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tetrisGame
{    
    public partial class Form1 : Form
    {
        int rowNo = 20;
        int colNo = 10;
        
        int[,] tmpMatrix = new int[4, 4]; // create a sample of shape
        int[,] currentMatrix = new int[4, 4]; // create a sample of shape

        int currentShapeCol;
        int currentShapeRow;        

        bool newShape = false;
        bool collision = false;
        int counterFullRow = 0;
        int scores = 0;

        enum userPress
        {            
            keyLeft = 1,
            keyRight,
            keyDown,
            keyUp
        };

        blockShape sampleShape;  // next shape 
        blockShape currentShape; // current shape

        Random randomShapeNumber = new Random();

        int currentFullRow;        

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // generate default 10 columns, 20 rows
            // block shapes : I, O, T, S, Z, J, L
            int top = 40;
            int left = 230;
            int widthOf1Label = 20;
            int heightOf1Label = 20;            

            // create matrix 10x20 
            for (int i=1; i <= rowNo; i++)
            {
                for (int j=1; j <= colNo; j++)
                {
                    // create a label
                    Label tmp = new Label();

                    create1Label(tmp, top + (i-1) * heightOf1Label, left + (j-1) * widthOf1Label, widthOf1Label, heightOf1Label, "lbl" + i.ToString("00") + j.ToString("00"));
                    tmp.BackColor = Color.LightGray;
                    // update the tag value for debug problem
                    //tmp.Text = tmp.Tag.ToString();
                    Controls.Add(tmp);
                }
            }

            /*
            // create number of col
            for (int i = 1; i < 11; i++)
            {
                // create a label
                Label tmp = new Label();

                create1Label(tmp, top - 25, left + (i - 1) * widthOf1Label, widthOf1Label + 1, heightOf1Label, "colNo" + i.ToString("00"));
                tmp.BackColor = Color.Empty;
                tmp.Text = i.ToString();
                Controls.Add(tmp);
            }

            // create number of row
            for (int i = 1; i < 21; i++)
            {
                // create a label
                Label tmp = new Label();

                create1Label(tmp, top + (i - 1) * heightOf1Label, left - 30, widthOf1Label + 5, heightOf1Label, "rowNo" + i.ToString("00"));
                tmp.BackColor = Color.Empty;
                tmp.Text = i.ToString();
                Controls.Add(tmp);
            }
            */

            // create text "Next Shape is :"
            Label header = new Label();
            header.Text = "Next shape is : ";
            header.Top = 35;
            header.Left = 110;
            Controls.Add(header);

            // create next shape
            for (int m = 1; m < 5; m++)
            {
                for (int n = 1; n < 5; n++)
                {
                    // create a label
                    Label tmp = new Label();

                    create1Label(tmp, 60 + (m - 1) * heightOf1Label, 110 + (n - 1) * widthOf1Label, widthOf1Label, heightOf1Label, "s" + m.ToString("00") + n.ToString("00"));
                    tmp.BorderStyle = BorderStyle.None;
                    Controls.Add(tmp);                    
                }
            }

            // create score record
            Label scoresText = new Label();            
            scoresText.Text = "Total Score:";
            scoresText.Top = 160;
            scoresText.Left = 110;
            Controls.Add(scoresText);

            // create score record
            Label scores = new Label();
            scores.Name = "totalScore";
            scores.Text = "0";
            scores.Top = 180;
            scores.Left = 130;
            scores.Height = 30;
            scores.Font = new Font(scores.Font.FontFamily.Name, 24);
            scores.ForeColor = Color.Blue;
            Controls.Add(scores);
        }

        public void create1Label(Label tmp, int top, int left, int width, int height, string labelName)
        {
            tmp.Top = top;
            tmp.Left = left;
            tmp.Width = width;
            tmp.Height = height;
            tmp.Name = labelName;
            //tmp.BorderStyle = BorderStyle.FixedSingle;
            tmp.BorderStyle = BorderStyle.Fixed3D;
            tmp.Tag = 0;
            //tmp.Text = tmp.Tag.ToString();                   
            //tmp.Text = tmp.Tag.ToString(); // display tag for debugging
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // rotate the shape
            sampleShape.rotateRight90();
            button2.PerformClick();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // clear color of the sample grid
            for (int m = 1; m < 5; m++)
            {
                for (int n = 1; n < 5; n++)
                {                    
                        Label tmpLabel = (Label)Controls["s" + m.ToString("00") + n.ToString("00")];
                        tmpLabel.BackColor = Color.Empty;                    
                }
            }

            // get color to be drawn
            foreach (RadioButton item in groupBox1.Controls)
            {
                if (item.Checked)
                {
                    sampleShape.setShape(Int16.Parse(item.Tag.ToString()));
                    tmpMatrix = sampleShape.updateBlockColor();
                }
            }

            // draw the shape in sample grid
            for (int m = 1; m < 5; m++)
            {
                for (int n = 1; n < 5; n++)
                {
                    if (tmpMatrix[m-1,n-1] == 1)
                    {
                        Label tmpLabel = (Label)Controls["s" + m.ToString("00") + n.ToString("00")];
                        tmpLabel.BackColor = sampleShape.getColor();
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1.PerformClick();
            button4.Enabled = true;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
            this.Text = "Tetris - by Si Thao - built on 20170314";
            radioButton8.Checked = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // START the game
            if (radioButton8.Checked)
            {
                timer1.Interval = 500;
            }
            if (radioButton9.Checked)
            {
                timer1.Interval = 300;
            }
            if (radioButton10.Checked)
            {
                timer1.Interval = 100;
            }

            timer1.Enabled = true;
            newShape = false;

            randomNextShape();
            richTextBox1.Clear();

            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = true;
            button7.Enabled = false;

            radioButton8.Enabled = false;
            radioButton9.Enabled = false;
            radioButton10.Enabled = false;            

            radioButton1.Focus();        
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Game control            
            if (!newShape)
            {
                // create new shape
                newShape = true;
                currentShapeCol = 4; // default position when new shape starts
                currentShapeRow = 1; // default position when new shape starts

                currentShape = new blockShape();                

                currentShape.setShape(sampleShape.getShape());
                currentShape.setCurrentOrientation(sampleShape.getCurrentOrientation());
                currentShape.setCurrentPos(currentShapeRow, currentShapeCol);

                currentMatrix = currentShape.updateBlockColor();

                // draw shape with color, referent point top-left
                draw1Shape(currentShapeRow, currentShapeCol, currentShape, 1);
                //richTextBox1.Text += "new shape row=" + currentShapeRow + "_Col=" + currentShapeCol + "\n";

                Label tmpScore = (Label)Controls["totalScore"];
                tmpScore.Text = scores.ToString();

                //timer1.Enabled = false;
                //MessageBox.Show("continue");
                //timer1.Enabled = true;
            } // end new shape 
            else
            {               
                // check for collision before drop shape
                for (int mRow = 1; mRow < 5; mRow++)
                {   // row
                    for (int nCol = 1; nCol < 5; nCol++)
                    {   // column
                        if (currentMatrix[mRow - 1, nCol - 1] == 1)
                        {
                            string tmpLabelName;
                            Label tmpLabel;

                            tmpLabelName = "lbl" + (mRow - 1 + currentShapeRow + 1).ToString("00") + (nCol - 1 + currentShapeCol).ToString("00");
                            tmpLabel = (Label)Controls[tmpLabelName];

                            if (Int32.Parse(tmpLabel.Tag.ToString()) > 0)
                            {
                                // reach limit, can not move
                                collision = true;
                            }
                        }
                    }
                } // end check collision before drop shape

                if (!collision)
                {
                    // no collision  
                    // current referent point is (1,4)  

                    // get position of previous shape
                    currentShape.getCurrentPos(ref currentShapeRow, ref currentShapeCol);
                    //richTextBox1.Text += "before clear=" + currentShapeRow + "_col=" + currentShapeCol + "\n";

                    // clear the previous shape color                    
                    draw1Shape(currentShapeRow, currentShapeCol, currentShape, 0);

                    // get matrix with refer to original (0,0)
                    currentMatrix = currentShape.updateBlockColor();                    

                    if (currentShapeRow == 2)
                    {
                        randomNextShape();
                    }

                    currentShapeRow += 1; // increase row number by 1 (simulate shape drop by 1 cell)

                    // no action, drop and draw shape at new position                    
                    draw1Shape(currentShapeRow, currentShapeCol, currentShape, 1);                    
                    currentShape.setCurrentPos(currentShapeRow, currentShapeCol);

                    //richTextBox1.Text += "after drop=" + currentShapeRow + "_col=" + currentShapeCol + "\n";                    
                } // end no collisio, start to drop shape

                // check if current shape drop to the bottom and can not drop more                
                if ((currentShape.getShapeMaxRow() - 1 + currentShapeRow) == rowNo || collision)
                {
                    // reached last rowNo or reached last possible empty cell
                    int tmpRow = 0, tmpCol = 0;

                    currentShape.getCurrentPos(ref tmpRow, ref tmpCol);

                    if (collision && tmpRow == 1)
                    {
                        timer1.Enabled = false;
                        //richTextBox1.Text += "Game over, tmpRow=" + tmpRow + "curShapeRow=" + currentShapeRow + "\n";
                        MessageBox.Show(" GAME OVER !!!");
                        button5.Enabled = true;
                    }

                    //update background block occupied by the currentshape
                    currentMatrix = currentShape.updateBlockColor();

                    for (int mRow = 1; mRow < 5; mRow++)
                    {   // row
                        for (int nCol = 1; nCol < 5; nCol++)
                        {   // column
                            if (currentMatrix[mRow - 1, nCol - 1] == 1)
                            {
                                string tmpLabelName;
                                Label tmpLabel;

                                tmpLabelName = "lbl" + (mRow - 1 + currentShapeRow).ToString("00") + (nCol - 1 + currentShapeCol).ToString("00");
                                tmpLabel = (Label)Controls[tmpLabelName];
                                tmpLabel.Tag = currentShape.getShape();
                                // update the tag value for debug problem
                                //tmpLabel.Text = tmpLabel.Tag.ToString();
                            }
                        }
                    }

                    // update Tag value to detect collision
                    for (int m = 1; m <= rowNo; m++)
                    {
                        for (int n = 1; n <= colNo; n++)
                        {
                            string tmpLabelName = "lbl" + m.ToString("00") + n.ToString("00");
                            Label tmpLabel = (Label)Controls[tmpLabelName];
                            // update the tag value for debug problem
                            //tmpLabel.Text = tmpLabel.Tag.ToString();
                        }
                    }

                    //timer1.Enabled = false;
                    //MessageBox.Show("before remove full rows");
                    //timer1.Enabled = true;

                    int counterCheckFull = 0;
                    while(counterCheckFull != 1)
                    { 
                        // start check full row and reset tag value = 0
                        for (int m = rowNo; m >= 1; m--)
                        {
                            counterFullRow = 0;
                        
                            if (m==1)
                            {
                                counterCheckFull = 1;
                            }
                            else
                            {
                                counterCheckFull = m;    
                            }

                            for (int n = 1; n <= colNo; n++)
                            {
                                string tmpLabelName = "lbl" + m.ToString("00") + n.ToString("00");
                                Label tmpLabel = (Label)Controls[tmpLabelName];

                                if (Int32.Parse(tmpLabel.Tag.ToString()) > 0)
                                {
                                    counterFullRow++;
                                }
                            }

                            // check if row is full, delete full row by reset tag value to 0
                            if (counterFullRow == colNo)
                            {
                                currentFullRow = m; // mark current full row number
                                scores++;

                                for (int i = m; i >= 1; i--)
                                {
                                    for (int j = 1; j <= colNo; j++)
                                    {
                                        if (i != 1)
                                        {
                                            // not top row, copy top to bottom
                                            string top = "lbl" + (i - 1).ToString("00") + j.ToString("00");
                                            Label tmptop = (Label)Controls[top];

                                            string bottom = "lbl" + i.ToString("00") + j.ToString("00");
                                            Label tmpbtm = (Label)Controls[bottom];

                                            // update the tag value for debug problem
                                            //tmpbtm.Text = tmptop.Text;
                                            tmpbtm.Tag = tmptop.Tag;

                                            tmpbtm.Update();
                                            tmptop.Update();

                                            switch (Int32.Parse(tmpbtm.Tag.ToString()))
                                            {
                                                case 1:
                                                    tmpbtm.BackColor = Color.Aqua;
                                                    break;
                                                case 2:
                                                    tmpbtm.BackColor = Color.Yellow;
                                                    break;
                                                case 3:
                                                    tmpbtm.BackColor = Color.DarkOrchid;
                                                    break;
                                                case 4:
                                                    tmpbtm.BackColor = Color.Lime;
                                                    break;
                                                case 5:
                                                    tmpbtm.BackColor = Color.Red;
                                                    break;
                                                case 6:
                                                    tmpbtm.BackColor = Color.Blue;
                                                    break;
                                                case 7:
                                                    tmpbtm.BackColor = Color.Orange;
                                                    break;
                                                default:
                                                    tmpbtm.BackColor = Color.LightGray;
                                                    break;
                                            }

                                            tmpbtm.Update();
                                            tmptop.Update();
                                        }
                                        else
                                        {
                                            // top row, create new after shift down
                                            string topRow = "lbl" + i.ToString("00") + j.ToString("00");
                                            Label lbltopRow = (Label)Controls[topRow];
                                            lbltopRow.Tag = 0;
                                            // update the tag value for debug problem
                                            //lbltopRow.Text = "0";
                                            lbltopRow.Update();
                                        }
                                    }
                                }
                                    break;  // exit for loop to loop again after remove 1 full row 
                            } // end if full row meet                                                
                        } // end for loop check full row from bottom to top
                    } // end while
                    newShape = false;
                    collision = false;                    

                } // end reached bottom or collision                                                                               
            } // end else - not new shape (dropping down)     
        } // end timer click

        private void button5_Click(object sender, EventArgs e)
        {            
            // clear the matrix
            for (int m = 1; m <= rowNo; m++)
            {
                for (int n = 1; n <= colNo; n++)
                {               
                    string tmpLabelName = "lbl" + m.ToString("00") + n.ToString("00");
                    Label tmpLabel = (Label)Controls[tmpLabelName];
                    tmpLabel.BackColor = Color.LightGray;
                    tmpLabel.Tag = 0;
                }
            }

            // clear next shape
            for (int m = 1; m < 5; m++)
            {
                for (int n = 1; n < 5; n++)
                {
                    string tmpLabelName = "s" + m.ToString("00") + n.ToString("00");
                    Label tmpLabel = (Label)Controls[tmpLabelName];
                    tmpLabel.BackColor = Color.Empty;
                    tmpLabel.Tag = 0;
                }
            }

            collision = false;            

            timer1.Enabled = false;

            button4.Enabled = true;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;

            radioButton8.Enabled = true;
            radioButton9.Enabled = true;
            radioButton10.Enabled = true;

            Label tmpScore = (Label)Controls["totalScore"];
            scores = 0;
            tmpScore.Text = "0";
        }      

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //capture up arrow key
            if (keyData == Keys.Up)
            {
                //MessageBox.Show("You pressed Up arrow key");                
                return true;
            }

            //capture down arrow key
            if (keyData == Keys.Down)
            {
                /*
                //MessageBox.Show("You pressed Down arrow key");                
                int tmpRow = 0, tmpCol = 0;
                currentShape.getCurrentPos(ref tmpRow, ref tmpCol);
                //richTextBox1.Text += "before Down row="+tmpRow + "_col=" + tmpCol +"\n";

                draw1Shape(tmpRow, tmpCol, currentShape, 0); // clear current shape
                draw1Shape(tmpRow, tmpCol, currentShape, 4); // draw shape at down position
                currentShape.setCurrentPos(tmpRow+1, tmpCol);

                int tmpRow2 = 0, tmpCol2 = 0;
                currentShape.getCurrentPos(ref tmpRow2, ref tmpCol2);
                //richTextBox1.Text += "after Down row=" + tmpRow2 + "_col=" + tmpCol2 + "\n";
                */
                return true;
            }

            //capture left arrow key
            if (keyData == Keys.Left)
            {
                //MessageBox.Show("You pressed Left arrow key");                
                int tmpRow = 0, tmpCol = 0;
                currentShape.getCurrentPos(ref tmpRow, ref tmpCol);
                //richTextBox1.Text += "before Left row=" + tmpRow + "_col=" + tmpCol + "\n";

                if (tmpCol == 1)
                {
                    // reach Left border, can not move left any more
                }
                else
                {
                    draw1Shape(tmpRow, tmpCol, currentShape, 0);
                    draw1Shape(tmpRow, tmpCol, currentShape, 2);
                    currentShape.setCurrentPos(tmpRow, tmpCol - 1);
                }
                
                int tmpRow2 = 0, tmpCol2 = 0;
                currentShape.getCurrentPos(ref tmpRow2, ref tmpCol2);
                //richTextBox1.Text += "after Left row=" + tmpRow2 + "_col=" + tmpCol2 + "\n";

                return true;
            }

            //capture right arrow key
            if (keyData == Keys.Right)
            {
                //MessageBox.Show("You pressed Right arrow key");                
                int tmpRow = 0, tmpCol = 0;
                currentShape.getCurrentPos(ref tmpRow, ref tmpCol);
                //richTextBox1.Text += "before Right row=" + tmpRow + "_col=" + tmpCol + "\n";

                if ((tmpCol + currentShape.getShapeMaxWidth() - 1) == colNo)
                {
                    // reach Right border, can not move Right any more
                }
                else
                {
                    draw1Shape(tmpRow, tmpCol, currentShape, 0);
                    draw1Shape(tmpRow, tmpCol, currentShape, 3);
                    currentShape.setCurrentPos(tmpRow, tmpCol + 1);
                }
               
                int tmpRow2 = 0, tmpCol2 = 0;
                currentShape.getCurrentPos(ref tmpRow2, ref tmpCol2);
                //richTextBox1.Text += "after Right row=" + tmpRow2 + "_col=" + tmpCol2 + "\n";

                return true;
            }
           
            if (keyData == Keys.Space)
            {                           
                int tmpRow = 0, tmpCol = 0;
                currentShape.getCurrentPos(ref tmpRow, ref tmpCol);
                //richTextBox1.Text += "before Rotate row=" + tmpRow + "_col=" + tmpCol + "\n";
                
                draw1Shape(tmpRow, tmpCol, currentShape, 0);
                
                currentShape.rotateRight90();
                
                draw1Shape(tmpRow, tmpCol, currentShape, 1);
                
                int tmpRow2 = 0, tmpCol2 = 0;
                currentShape.getCurrentPos(ref tmpRow2, ref tmpCol2);                
                //richTextBox1.Text += "after Rotate row=" + tmpRow2 + "_col=" + tmpCol2 + "\n";
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void draw1Shape(int currShapeRow, int currShapeCol, blockShape currShape, int operationMode)
        {
            /*
                int operationMode :
                0 : clear color of shape
                1 : paint color of shape
                2 : move left
                3 : move right
                4 : move down
            */
            // draw shape with color and clear the previous color after dropping down
            for (int mRow = 1; mRow < 5; mRow++)
            {   // row
                for (int nCol = 1; nCol < 5; nCol++)
                {   // column
                    if (currentMatrix[mRow - 1, nCol - 1] == 1)
                    {
                        string tmpLabelName;
                        Label tmpLabel;

                        if (operationMode == 1)
                        {   // paint color at exact position, no movement
                            tmpLabelName = "lbl" + (mRow - 1 + currShapeRow).ToString("00") + (nCol - 1 + currShapeCol).ToString("00");
                            tmpLabel = (Label)Controls[tmpLabelName];
                            tmpLabel.BackColor = currShape.getColor();                           
                            tmpLabel.Update();
                        }
                        else if (operationMode == 0)
                        {   // delete current shape
                            tmpLabelName = "lbl" + (mRow - 1 + currShapeRow).ToString("00") + (nCol - 1 + currShapeCol).ToString("00");
                            tmpLabel = (Label)Controls[tmpLabelName];                            
                            tmpLabel.BackColor = Color.LightGray;                            
                            tmpLabel.Update();
                        }
                        else if (operationMode == 2)
                        {   // move left
                            tmpLabelName = "lbl" + (mRow - 1 + currShapeRow).ToString("00") + (nCol - 2 + currShapeCol).ToString("00");
                            tmpLabel = (Label)Controls[tmpLabelName];
                            tmpLabel.BackColor = currShape.getColor();                            
                            tmpLabel.Update();
                        }
                        else if (operationMode == 3)
                        {   // move right
                            tmpLabelName = "lbl" + (mRow - 1 + currShapeRow).ToString("00") + (nCol + currShapeCol).ToString("00");
                            tmpLabel = (Label)Controls[tmpLabelName];
                            tmpLabel.BackColor = currShape.getColor();                           
                            tmpLabel.Update();
                        }
                        else if (operationMode == 4)
                        {   // move down
                            tmpLabelName = "lbl" + (mRow + currShapeRow).ToString("00") + (nCol - 1 + currShapeCol).ToString("00");
                            tmpLabel = (Label)Controls[tmpLabelName];
                            tmpLabel.BackColor = currShape.getColor();                            
                            tmpLabel.Update();
                        }
                    } // end check shape cell position
                } // end column loop
            } // end row loop
        }

        public void randomNextShape()
        {
            //richTextBox1.Text += "start randomNextShape() \n";
            sampleShape = new blockShape();

            // clear color of the sample grid
            for (int m = 1; m < 5; m++)
            {
                for (int n = 1; n < 5; n++)
                {
                    Label tmpLabel = (Label)Controls["s" + m.ToString("00") + n.ToString("00")];
                    tmpLabel.BackColor = Color.Empty;
                }
            }

           int tmpRandomShape = randomShapeNumber.Next(1, 8);
           int tmpRandomRotation = randomShapeNumber.Next(0, 4); // 0*90=0, 1*90=90, 2*90=180, 3*90 = 270
                      
           sampleShape.setShape(tmpRandomShape);
           sampleShape.setCurrentOrientation(tmpRandomRotation * 90);

           tmpMatrix = sampleShape.updateBlockColor();
               

            // draw the shape in sample grid
            for (int m = 1; m < 5; m++)
            {
                for (int n = 1; n < 5; n++)
                {
                    if (tmpMatrix[m - 1, n - 1] == 1)
                    {
                        Label tmpLabel = (Label)Controls["s" + m.ToString("00") + n.ToString("00")];
                        tmpLabel.BackColor = sampleShape.getColor();
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Pause game            
            timer1.Enabled = false;
            button5.Enabled = true;
            button6.Enabled = false;
            button7.Enabled = true;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Resume game            
            timer1.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = false;
        }

    } // end class Form1

    public class blockShape
    {
        private int currentPositionOrientation; // 0, 90, 180, 270        
        private enum shapeType { I = 1, O, T, S, Z, J, L };
        private int currentShape;
        private int currentRefRow;  // left position of 4x4 square
        private int currentRefCol;  // top position of 4x4 square
        private int shapeMaxHeight; // number of rows occupied by the shape to calculate reach bottom
        private int shapeMaxWidth;  // number of columns occupied by the shape to calculate reach Right border

        public void setCurrentPos(int row, int col)
        {
            this.currentRefRow = row;
            this.currentRefCol = col;
        }

        public void getCurrentPos(ref int row, ref int col)
        {
            row = this.currentRefRow;
            col = this.currentRefCol;
        }

        public void setShape(int setShape)
        {
            this.currentShape = setShape;
        }

        public int getShape()
        {
            return this.currentShape;
        }

        public int[,] updateBlockColor()
        {
            int[,] tmpblock = new int[4, 4];
            int currOrt = getCurrentOrientation();

            switch (currentShape)
            {
                case (int)shapeType.I:
                    // I type
                    if (currOrt == 0 || currOrt == 180)
                    {
                        tmpblock[0, 0] = 1;
                        tmpblock[0, 1] = 1;
                        tmpblock[0, 2] = 1;
                        tmpblock[0, 3] = 1;
                        shapeMaxHeight = 1;
                        shapeMaxWidth = 4;
                    }
                    else
                    {
                        tmpblock[0, 0] = 1;
                        tmpblock[1, 0] = 1;
                        tmpblock[2, 0] = 1;
                        tmpblock[3, 0] = 1;
                        shapeMaxHeight = 4;
                        shapeMaxWidth = 1;
                    }
                    break;
                case (int)shapeType.O:
                    // O type
                    {
                        tmpblock[0, 0] = 1;
                        tmpblock[0, 1] = 1;
                        tmpblock[1, 0] = 1;
                        tmpblock[1, 1] = 1;
                        shapeMaxHeight = 2;
                        shapeMaxWidth = 2;
                    }
                    break;
                case (int)shapeType.T:
                    // T type
                    if (currOrt == 0)
                    {
                        tmpblock[0, 0] = 1;
                        tmpblock[0, 1] = 1;
                        tmpblock[0, 2] = 1;
                        tmpblock[1, 1] = 1;
                        shapeMaxHeight = 2;
                        shapeMaxWidth = 3;
                    }
                    else if (currOrt == 90)
                    {
                        tmpblock[0, 1] = 1;
                        tmpblock[1, 0] = 1;
                        tmpblock[1, 1] = 1;
                        tmpblock[2, 1] = 1;
                        shapeMaxHeight = 3;
                        shapeMaxWidth = 2;
                    }
                    else if (currOrt == 180)
                    {
                        tmpblock[0, 1] = 1;
                        tmpblock[1, 0] = 1;
                        tmpblock[1, 1] = 1;
                        tmpblock[1, 2] = 1;
                        shapeMaxHeight = 2;
                        shapeMaxWidth = 3;
                    }
                    else
                    {
                        tmpblock[0, 0] = 1;
                        tmpblock[1, 0] = 1;
                        tmpblock[1, 1] = 1;
                        tmpblock[2, 0] = 1;
                        shapeMaxHeight = 3;
                        shapeMaxWidth = 2;
                    }
                    break;
                case (int)shapeType.S:
                    // S type
                    if (currOrt == 0 || currOrt == 180)
                    {
                        tmpblock[0, 1] = 1;
                        tmpblock[0, 2] = 1;
                        tmpblock[1, 0] = 1;
                        tmpblock[1, 1] = 1;
                        shapeMaxHeight = 2;
                        shapeMaxWidth = 3;
                    }
                    else
                    {
                        tmpblock[0, 0] = 1;
                        tmpblock[1, 0] = 1;
                        tmpblock[1, 1] = 1;
                        tmpblock[2, 1] = 1;
                        shapeMaxHeight = 3;
                        shapeMaxWidth = 2;
                    }
                    break;
                case (int)shapeType.Z:
                    // Z type
                    if (currOrt == 0 || currOrt == 180)
                    {
                        tmpblock[0, 0] = 1;
                        tmpblock[0, 1] = 1;
                        tmpblock[1, 1] = 1;
                        tmpblock[1, 2] = 1;
                        shapeMaxHeight = 2;
                        shapeMaxWidth = 3;
                    }
                    else
                    {
                        tmpblock[0, 1] = 1;
                        tmpblock[1, 0] = 1;
                        tmpblock[1, 1] = 1;
                        tmpblock[2, 0] = 1;
                        shapeMaxHeight = 3;
                        shapeMaxWidth = 2;
                    }
                    break;
                case (int)shapeType.J:
                    // J type
                    if (currOrt == 0)
                    {
                        tmpblock[0, 1] = 1;
                        tmpblock[1, 1] = 1;
                        tmpblock[2, 0] = 1;
                        tmpblock[2, 1] = 1;
                        shapeMaxHeight = 3;
                        shapeMaxWidth = 2;
                    }
                    else if (currOrt == 90)
                    {
                        tmpblock[0, 0] = 1;
                        tmpblock[1, 0] = 1;
                        tmpblock[1, 1] = 1;
                        tmpblock[1, 2] = 1;
                        shapeMaxHeight = 2;
                        shapeMaxWidth = 3;
                    }
                    else if (currOrt == 180)
                    {
                        tmpblock[0, 0] = 1;
                        tmpblock[0, 1] = 1;
                        tmpblock[1, 0] = 1;
                        tmpblock[2, 0] = 1;
                        shapeMaxHeight = 3;
                        shapeMaxWidth = 2;
                    }
                    else
                    {
                        tmpblock[0, 0] = 1;
                        tmpblock[0, 1] = 1;
                        tmpblock[0, 2] = 1;
                        tmpblock[1, 2] = 1;
                        shapeMaxHeight = 2;
                        shapeMaxWidth = 3;
                    }
                    break;
                case (int)shapeType.L:
                    // L type
                    if (currOrt == 0)
                    {
                        tmpblock[0, 0] = 1;
                        tmpblock[1, 0] = 1;
                        tmpblock[2, 0] = 1;
                        tmpblock[2, 1] = 1;
                        shapeMaxHeight = 3;
                        shapeMaxWidth = 2;
                    }
                    else if (currOrt == 90)
                    {
                        tmpblock[0, 0] = 1;
                        tmpblock[0, 1] = 1;
                        tmpblock[0, 2] = 1;
                        tmpblock[1, 0] = 1;
                        shapeMaxHeight = 2;
                        shapeMaxWidth = 3;
                    }
                    else if (currOrt == 180)
                    {
                        tmpblock[0, 0] = 1;
                        tmpblock[0, 1] = 1;
                        tmpblock[1, 1] = 1;
                        tmpblock[2, 1] = 1;
                        shapeMaxHeight = 3;
                        shapeMaxWidth = 2;
                    }
                    else
                    {
                        tmpblock[0, 2] = 1;
                        tmpblock[1, 0] = 1;
                        tmpblock[1, 1] = 1;
                        tmpblock[1, 2] = 1;
                        shapeMaxHeight = 2;
                        shapeMaxWidth = 3;
                    }
                    break;
                default:
                    break;
            }

            return tmpblock;
        }

        public int getCurrentOrientation()
        {
            return this.currentPositionOrientation;
        }

        public void setCurrentOrientation(int currOrient)
        {
            this.currentPositionOrientation = currOrient;
        }

        public void rotateRight90()
        {
            int tmpCurr = this.getCurrentOrientation();

            if (tmpCurr == 270)
            {
                this.setCurrentOrientation(0);
            }
            else
            {
                this.setCurrentOrientation(tmpCurr + 90);
            }
        }

        public Color getColor()
        {
            switch (getShape())
            {
                case (int)shapeType.I:
                    return Color.Aqua;
                case (int)shapeType.O:
                    return Color.Yellow;
                case (int)shapeType.T:
                    return Color.DarkOrchid;
                case (int)shapeType.S:
                    return Color.Lime;
                case (int)shapeType.Z:
                    return Color.Red;
                case (int)shapeType.J:
                    return Color.Blue;
                case (int)shapeType.L:
                    return Color.Orange;
                default:
                    return Color.Empty;
            }
        }

        public int getShapeMaxRow()
        {
            return this.shapeMaxHeight;
        }

        public int getShapeMaxWidth()
        {
            return this.shapeMaxWidth;
        }
    
    } // end class blockShape
}
