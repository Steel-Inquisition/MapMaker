using System;
using System.Collections.Generic;
using System.IO;  // include the System.IO namespace
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MapMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// 
    /// this code is wonky but is just used to quickly make the map
    /// 
    /// This was used to make the maps
    /// however this was before I learned about storing itself in lists and all that, so there is some wonky stuff
    /// also before I learned about JSON
    /// 
    /// 
    /// </summary>
    public partial class MainWindow : Window
    {


        // Create the Wall
        public List<WallMaker> wallStats = new();

        // Create the Objects
        public List<Rectangle> objectStats = new();

        // Total Map in dictionary
        public Dictionary<int, string> totalMap = new();

        // room type
        public List<string> roomType = new();

        // size of room
        public int totalMapSize = 100;


        // cretae clicker
        public Clicker Clicker = new("wall", "tile");

        // get current room
        public int currentRoom = 0;


        public MainWindow()
        {
            InitializeComponent();
        }


        public void SearchFile()
        {

            // find files of map based on input
            string totalMapText = System.IO.File.ReadAllText(@$"data-files/{InputMap.Text}.txt");
            string[] totalMapSplit = Regex.Split(totalMapText, @"\r\n+");

            // find files of room based on input
            string totalRoomType = System.IO.File.ReadAllText(@$"data-files/{InputMap.Text}Settings.txt");
            string[] totalRoomSplit = Regex.Split(totalRoomType, @"\r\n+");

            // add this to the total map
            for (int i = 0; i < totalMapSize; i++)
            {
                totalMap.Add(i, totalMapSplit[i]);
                roomType.Add(totalRoomSplit[i]);
            }

        }

        // get the file wanted
        private void ButtonLoad(object sender, RoutedEventArgs e)
        {

            // get file based on input from text
            SearchFile();

            // change visibility
            LoadingCanvas.Visibility = Visibility.Hidden;

            // Create items of the canvs
            CreateItem(PlayerSpace, "cross", 0);

            // create the objects of the canvas
            CreateObjectRoom(ObjectSpace, "circle", 1);

            // get room text
            RoomType.Text = roomType[currentRoom];
        }


        public void CreateItem(Canvas Space, string getImage, int start)
        {

            String dictionary = totalMap[currentRoom];
            String[] dictionaryWords = Regex.Split(dictionary, @",+");

            // Create map. This will be used as the basis for the place where the player moves
            List<string> room = new List<string>();

            for (int i = start; i < dictionaryWords.Length; i += 2)
            {

                room.Add(dictionaryWords[i]);
            }

            // SET UP ROOM

            int col = 0;
            int row = 0;
            int section = 20;
            int generateRoom = 0;

            int wallSize = 25;


            // Idea: Already create the rectangle floor, just set the FILL style of the floor instead of creating a new rectangle each time

            while (generateRoom < room.Count)
            {

                if (generateRoom < section) // If this part of the array is bellow the length of the map (20)
                {

                    if (room[generateRoom] == "0")
                    {
                        row++; // If this part of the array is 0

                        wallStats.Add(new WallMaker(100, 25, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "grass"));
                    }
                    else if (room[generateRoom] == "1") // If the given part of the map has a 1
                    {
                        // Increase the row
                        row++;

                        wallStats.Add(new WallMaker(100, 25, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "wall"));

                    }
                    else if (room[generateRoom] == "2")
                    {
                        // Increase the row
                        row++;

                        wallStats.Add(new WallMaker(100, 25, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "dirt"));
                    }
                    else if (room[generateRoom] == "3")
                    {
                        // Increase the row
                        row++;

                        wallStats.Add(new WallMaker(100, 25, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "wood"));
                    }
                    else if (room[generateRoom] == "WATER")
                    {
                        // Increase the row
                        row++;

                        wallStats.Add(new WallMaker(100, 25, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "water"));
                    }


                    generateRoom++; // Get to the next part of the array

                }
                else
                {
                    // When reading the array goes farther than the length of the map (10) switch to a new row
                    row = 0;
                    col++;
                    section += 20;
                }

            }
        }


        public void CreateObjectRoom(Canvas Space, string getImage, int start)
        {

            String dictionary = totalMap[currentRoom];
            String[] dictionaryWords = Regex.Split(dictionary, @",+");

            // Create map. This will be used as the basis for the place where the player moves
            List<string> room = new List<string>();

            for (int i = start; i < dictionaryWords.Length; i += 2)
            {
                room.Add(dictionaryWords[i]);
            }

            // SET UP ROOM

            int col = 0;
            int row = 0;
            int section = 20;
            int generateRoom = 0;

            int wallSize = 25;


            // Idea: Already create the rectangle floor, just set the FILL style of the floor instead of creating a new rectangle each time

            while (generateRoom < room.Count)
            {

                if (generateRoom < section) // If this part of the array is bellow the length of the map (20)
                {

                    if (room[generateRoom] == "ENEMY") // If the given part of the map has a 1
                    {
                        // Increase the row
                        row++;

                        wallStats.Add(new WallMaker(100, 25, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "enemy"));

                    }
                    else if (room[generateRoom] == "SHOOTER") // If the given part of the map has a 1
                    {
                        // Increase the row
                        row++;

                        wallStats.Add(new WallMaker(100, 25, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "shooter"));

                    }
                    else if (room[generateRoom] == "DOORUP")
                    {
                        row++;

                        wallStats.Add(new WallMaker(100, 50, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "doorup"));

                    }
                    else if (room[generateRoom] == "DOORSIDE")
                    {
                        row++;

                        wallStats.Add(new WallMaker(100, 25, 50, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "doorside"));

                    }
                    else if (room[generateRoom] == "STATUE")
                    {
                        row++;

                        wallStats.Add(new WallMaker(100, 25, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "statue"));

                    }
                    else if (room[generateRoom] == "TABLE")
                    {
                        row++;

                        wallStats.Add(new WallMaker(100, 25, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "table"));

                    }
                    else if (room[generateRoom] == "CHAIR")
                    {
                        row++;

                        wallStats.Add(new WallMaker(100, 25, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "chair"));

                    }
                    else if (room[generateRoom] == "2")
                    {
                        // Increase the row
                        row++;

                        wallStats.Add(new WallMaker(1, 25, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "coin"));
                    }
                    else if (room[generateRoom] == "3")
                    {
                        // Increase the row
                        row++;

                        wallStats.Add(new WallMaker(1, 25, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "ammo"));
                    }
                    else if (room[generateRoom] == "4")
                    {
                        // Increase the row
                        row++;

                        wallStats.Add(new WallMaker(1, 25, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "holy_cross"));
                    }
                    else if (room[generateRoom] == "5")
                    {
                        // Increase the row
                        row++;

                        wallStats.Add(new WallMaker(1, 25, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "key"));
                    }
                    else if (room[generateRoom] == "6")
                    {
                        // Increase the row
                        row++;

                        wallStats.Add(new WallMaker(1, 25, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "bomb"));
                    }
                    else if (room[generateRoom] == "W") // weapon crate
                    {
                        // Increase the row
                        row++;

                        // Chnage this to a crate later on
                        wallStats.Add(new WallMaker(1, 25, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "crate"));
                    }
                    else if (room[generateRoom] == "B") // boon crate
                    {
                        row++;

                        // Chnage this to a crate later on
                        wallStats.Add(new WallMaker(1, 25, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "crate2"));
                    }
                    else if (room[generateRoom] == "H") // heal crate
                    {
                        row++;

                        // Chnage this to a crate later on
                        wallStats.Add(new WallMaker(1, 25, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "crate3"));
                    }
                    else if (room[generateRoom] == "WALL")
                    {
                        row++;

                        wallStats.Add(new WallMaker(1, 25, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "fakewall"));
                    }
                    else if (room[generateRoom] == "EXIT")
                    {
                        row++;

                        wallStats.Add(new WallMaker(1, 25, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "exit"));
                    }
                    else if (room[generateRoom] == "FROG")
                    {
                        row++;

                        wallStats.Add(new WallMaker(1, 25, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "frog"));
                    }
                    else if (room[generateRoom] == "FIREBALL")
                    {
                        row++;

                        wallStats.Add(new WallMaker(1, 25, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "fireball"));
                    }
                    else if (room[generateRoom] == "EYEBEAST")
                    {
                        row++;

                        wallStats.Add(new WallMaker(1, 25, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "Eye"));
                    }
                    else if (room[generateRoom] == "SOLDIER")
                    {
                        row++;

                        wallStats.Add(new WallMaker(1, 25, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "soldier"));
                    }
                    else if (room[generateRoom] == "SLIME")
                    {
                        row++;

                        wallStats.Add(new WallMaker(1, 25, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "slime"));
                    }
                    else
                    {
                        // Increase the row
                        row++;

                        wallStats.Add(new WallMaker(100, 25, 25, (row * wallSize) + 55, (col * wallSize) + 80, Space, wallStats.Count, "circle"));
                    }

                    generateRoom++; // Get to the next part of the array

                }
                else
                {
                    // When reading the array goes farther than the length of the map (10) switch to a new row
                    row = 0;
                    col++;
                    section += 20;
                }

            }
        }



        // Tutorial on click functions
        // https://youtu.be/BbWgUaS60yE


        // When clicking, change image
        private void PlayerSpace_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            // if click and the source is a rectangle
            if (e.OriginalSource is Rectangle)
            {

                // and the mode is tile
                if (Clicker.mode == "tile")
                {
                    // get the rectangle of the original source
                    Rectangle x = (Rectangle)e.OriginalSource;

                    // change its name
                    x.Name = Clicker.clickerType;

                    // change the image
                    ImageBrush image = new();
                    string fileName = $"data-files/images/{Clicker.clickerType}.png";
                    string fullPath = System.IO.Path.GetFullPath(fileName);
                    image.ImageSource = new BitmapImage(new Uri(fullPath));
                    x.Fill = image;
                }
                else
                {
                    // get the rectangle of the original source
                    Rectangle x = (Rectangle)e.OriginalSource;

                    // change its name
                    x.Name = Clicker.clickerType;

                    // change the image
                    ImageBrush image = new();
                    string fileName = $"data-files/images/{Clicker.clickerType}.png";
                    string fullPath = System.IO.Path.GetFullPath(fileName);
                    image.ImageSource = new BitmapImage(new Uri(fullPath));
                    x.Fill = image;


                }


            }
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {

            // get the content of the button
            var keyword = (e.Source as Button).Content.ToString();

            // the clicker is now this type and is a tile
            Clicker.clickerType = keyword;
            Clicker.mode = "tile";

            // no longer show object layer
            ObjectSpace.Visibility = Visibility.Hidden;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            // get the content of the button
            var keyword = (e.Source as Button).Content.ToString();

            // the clicker is now this type and is a object
            Clicker.clickerType = keyword;
            Clicker.mode = "object";

            // show object layer
            ObjectSpace.Visibility = Visibility.Visible;
        }


        // savethe room
        public void saveRoom(object sender, RoutedEventArgs e)
        {
            List<string> tileInRoom = new List<string>();
            List<string> tileInObject = new List<string>();
            string fullRoom = "";



            // get the tiles and add it to the tileInRoom list
            foreach (Rectangle x in PlayerSpace.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "wall")
                {

                    if ((string)x.Name == "grass")
                    {
                        tileInRoom.Add("0,");
                    }
                    else if ((string)x.Name == "wall")
                    {
                        tileInRoom.Add("1,");
                    }
                    else if (x.Name == "dirt")
                    {
                        tileInRoom.Add("2,");
                    }
                    else if (x.Name == "wood")
                    {
                        tileInRoom.Add("3,");
                    }
                    else if (x.Name == "water")
                    {
                        tileInRoom.Add("WATER,");
                    }
                    else
                    {
                        tileInRoom.Add("0,");
                    }
                }
            }

            // get the object and add it to the tileInObject list
            foreach (Rectangle x in ObjectSpace.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "wall")
                {

                    if ((string)x.Name == "enemy")
                    {
                        tileInObject.Add("ENEMY,");

                    }
                    else if ((string)x.Name == "shooter")
                    {
                        tileInObject.Add("SHOOTER,");

                    }
                    else if ((string)x.Name == "frog")
                    {
                        tileInObject.Add("FROG,");

                    }
                    else if ((string)x.Name == "fireball")
                    {
                        tileInObject.Add("FIREBALL,");

                    }
                    else if ((string)x.Name == "Eye")
                    {
                        tileInObject.Add("EYEBEAST,");

                    }
                    else if ((string)x.Name == "soldier")
                    {
                        tileInObject.Add("SOLDIER,");

                    }
                    else if ((string)x.Name == "slime")
                    {
                        tileInObject.Add("SLIME,");

                    }
                    else if ((string)x.Name == "coin")
                    {
                        tileInObject.Add("2,");
                    }
                    else if ((string)x.Name == "ammo")
                    {
                        tileInObject.Add("3,");
                    }
                    else if ((string)x.Name == "holy_cross")
                    {
                        tileInObject.Add("4,");
                    }
                    else if ((string)x.Name == "key")
                    {
                        tileInObject.Add("5,");
                    }
                    else if ((string)x.Name == "bomb")
                    {
                        tileInObject.Add("6,");
                    }
                    else if ((string)x.Name == "crate")
                    {
                        tileInObject.Add("W,");
                    }
                    else if ((string)x.Name == "crate2")
                    {
                        tileInObject.Add("B,");
                    }
                    else if ((string)x.Name == "crate3")
                    {
                        tileInObject.Add("H,");
                    }
                    else if ((string)x.Name == "random")
                    {
                        tileInObject.Add("8,");
                    }
                    else if ((string)x.Name == "fakewall")
                    {
                        tileInObject.Add("WALL,");
                    }
                    else if ((string)x.Name == "doorup")
                    {
                        tileInObject.Add("DOORUP,");
                    }
                    else if ((string)x.Name == "statue")
                    {
                        tileInObject.Add("STATUE,");
                    }
                    else if ((string)x.Name == "table")
                    {
                        tileInObject.Add("TABLE,");
                    }
                    else if ((string)x.Name == "chair")
                    {
                        tileInObject.Add("CHAIR,");
                    }
                    else if ((string)x.Name == "doorside")
                    {
                        tileInObject.Add("DOORSIDE,");
                    }
                    else if ((string)x.Name == "exit")
                    {
                        tileInObject.Add("EXIT,");
                    }
                    else
                    {
                        tileInObject.Add("0,");
                    }
                }
            }


            // add this to the string in revolving order
            for (int i = 0; i < 400; i++)
            {
                fullRoom += tileInRoom[i]; // tile
                fullRoom += tileInObject[i]; // object
            }


            // se this to the total map
            totalMap[currentRoom] = fullRoom;

            // Save Room Type
            roomType[currentRoom] = RoomType.Text;

            // Saving Full Room Type
            String FullRoomType = "";

            for (int i = 0; i < totalMapSize; i++)
            {
                FullRoomType += roomType[i] + "\r\n";
            }

            File.WriteAllText("mapSettings.txt", FullRoomType);

            // Saving Full Map based on room

            String FullMapSave = "";

            for (int i = 0; i < totalMapSize; i++)
            {
                FullMapSave += totalMap[i] + "\r\n";
            }

            File.WriteAllText("map.txt", FullMapSave);  // Create a file and write the content of writeText to it
        }


        // Get a new room
        private void NewRoomButton_Click(object sender, RoutedEventArgs e)
        {

            // clear the canvas
            PlayerSpace.Children.Clear();
            ObjectSpace.Children.Clear();

            // get the current room
            currentRoom = Convert.ToInt16(GetMap.Text);

            // create the new item layer
            CreateItem(PlayerSpace, "cross", 0);

            // create the new object layer
            CreateObjectRoom(ObjectSpace, "circle", 1);

            // get the room type text
            RoomType.Text = roomType[currentRoom];

        }
    }


    // clicker class
    public class Clicker
    {
        public string clickerType = "";
        public string mode = "";

        public Clicker(string getClickerType, string getModeType)
        {
            this.clickerType = getClickerType;
            this.mode = getModeType;
        }
    }

    // wall maker class
    public class WallMaker
    {
        public double health;
        public int totalWall;
        public string name;

        public WallMaker(double getHealth, int height, int width, int x, int y, Canvas PlayerSpace, int totalWall, string getName)
        {
            this.health = getHealth;
            this.name = getName;

            var _ = new Draw($"wall", height, width, x, y, name, PlayerSpace);
        }

    }


    // draw the obejct
    public class Draw
    {
        public Draw(string tagName, double height, double width, double x, double y, string name, Canvas PlayerSpace)
        {

            ImageBrush image = new ImageBrush();

            string fileName = $"data-files/images/{name}.png";
            string fullPath = System.IO.Path.GetFullPath(fileName);

            image.ImageSource = new BitmapImage(new Uri(fullPath));

            Rectangle newRect = new Rectangle
            {
                Name = name,
                Tag = tagName,
                Height = height,
                Width = width,
                Fill = image
            };

            Canvas.SetLeft(newRect, x);
            Canvas.SetTop(newRect, y);

            PlayerSpace.Children.Add(newRect);


            // Collect Garbage
            GC.Collect(); // collect any unused resources for this game
        }


    }

    // fill the object
    public class FillDraw
    {
        public FillDraw(string imageName, Rectangle ThisObject)
        {
            ImageBrush image = new ImageBrush();

            string fileName = $"data-files/images/{imageName}.png";
            string fullPath = System.IO.Path.GetFullPath(fileName);

            image.ImageSource = new BitmapImage(new Uri(fullPath));

            ThisObject.Fill = image;
        }
    }

    // draw the text block
    public class DrawTextBlock
    {
        public DrawTextBlock(string tagName, int height, int width, int x, int y, int size, string text, Canvas PlayerSpace)
        {
            TextBlock textBlock = new TextBlock
            {
                Tag = tagName,
                Height = height,
                Width = width,
                Text = text,
                FontSize = size
            };



            Canvas.SetLeft(textBlock, x);
            Canvas.SetTop(textBlock, y);

            PlayerSpace.Children.Add(textBlock);


            // Collect Garbage
            GC.Collect(); // collect any unused resources for this game
        }
    }




}
