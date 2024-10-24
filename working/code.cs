//Hide the Form
this.Hide();

//Create the Bitmap
Bitmap printscreen = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

//Create the Graphic Variable with Screen Dimensions
Graphics graphics = Graphics.FromIamge(printscreen as Image);

//Copy Image from the screen
graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);

//Create a Temporal Memory Stream for the Image
using (MemoryStream s - new MemoryStream()) {
    //save graphic variable to memory
    printscreen.Save(s, ImageFormat.Bmp);
    pictureBox1.Size = new System.Drawing.Size(this.Width, this.Height);

    //set the picture box with temporary stream
    pictureBox1.Image = Image.FromStream(s);
}

//Show Form
this.Show();

//Make the Cursor Cross
Cursor = Cursors.Cross;

//----------------------------------------------------------------

//These variables control the mouse position
int selectX;
int selectY;
int selectWidth;
int selectHeight;
public Pen selectPen;

//This variable controls when you start the right click
bool start = false;

//----------------------------------------------------------------

private void pictureBox1_MouseMove(object sender, MouseEventArgs e) {
    //validate if there is an image
    if (pictureBox1.Image == null) {
        return;
    }
    
    //validate if right-click was triggered
    if(start) {
        //refresh picture box
        pictureBox1.Refresh();
        
        //set corner square to mouse coordinates
        selectWidth = e.X - selectY;
        selectHeight = e.Y - selectY;

        //draw dotted rectangle
        pictureBox1.CreateGraphics().DrawRectangle(selectPen, selectX, selectY, selectWidth, selectHeight);
    }
}

//----------------------------------------------------------------

private void pictureBox1_MouseDown(object sender, MouseEventArgs e) {
    //validate when user right-click
    if (!start) {
        if (e.Button == System.Windows.Forms.MouseButtons.Left) {
            //starts coordinates for rectangle
            select = e.X;
            select = e.Y;
            selectPen = new Pen(Color.Blue, 1);
            selectPen.DashStyle = DashStyle.DashDotDot;
        }

        //refresh picture box
        pictureBox1.Refresh();

        //start control variable for draw rectangle
        start = true;
    }

    else {
        //validate if there is an image
        if (pictureBox1.Image == null) {
            return;
        }

        //same functionality when mouse is over
        if (e.Button == System.Windows.Forms.MouseButtons.Left) {
            pictureBox1.Refresh();
            selectWidth = e.X - selectX;
            selectHeight = e.Y - selectY;
            pictureBox1.CreateGraphics().DrawRectangle(selectPen, selectX, selectY, selectWidth, selectHeight);
        }
        start = false;

        //function save image to clipboard
         SaveToClipboard();
    }
}

//----------------------------------------------------------------

private void SaveToClipboard() {
    //validate if something selected
    if (selectWidth > 0) {
        Rectangle rect = new Rectangle(selectX, selectY, selectWidth, selectHeight);

        //create bitmap with original dimensions
        Bitmap OriginalImage = new Bitmap(pictureBox1.Image, pictureBox1.Width, pictureBox1.Height);

        //create graphic variable
        Graphics g = Graphics.FromImage(_img);

        //set graphic attributes
        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        g.DrawImage(OriginalImage, 0, 0, rect, GraphicsUnit.Pixel);

        //insert image stream into clipboard
        Clipboard.SetImage(_img);
    }

    //End Application
    Application.Exit();
}