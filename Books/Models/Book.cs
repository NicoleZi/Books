﻿using System;
using System.Data.SqlTypes;
using SQLite;

namespace Books.Models
{
    public class Book
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Autor {  get; set; }

        // Booleans
        public bool Favorite { get; set; } = false;
        public bool Bought { get; set; } = false;
        public bool Read { get; set; } = false;
        public bool Private { get; set; } = false;

        // About the Book
        public int Volume { get; set; } = 0;
        public string Universum { get; set; }
        public string Genre { get; set; }
        public DateTime ReleaseDate { get; set; } = DateTime.Today;

        //Extras
        public string Comment { get; set; }
        public float Rating { get; set; }
        public string GalleryFolder { get; set; }

        // TBR Elements
        public int TBRIndex { get; set; }
        public bool NotFirstIndex { get; set; } = true;
        public bool NotLastIndex { get; set; } = true;
    }
}
