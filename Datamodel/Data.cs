using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;

namespace Datamodel
{
    [Table("Items")]
    public class movDatabase
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        public string Tenphim { get; set; }
        public int Sotap { get; set; }

        public string link { get; set; }

        [MaxLength(200)]
        public int min { get; set; }

        public string img { get; set; }
        public string Heading { get; internal set; }
        public string SubHeading { get; internal set; }

        //public byte[] Array { get; set; }
        //public object Array { get; internal set; }

        public movDatabase()
        {

        }

        public static explicit operator string(movDatabase v)
        {
            throw new NotImplementedException();
        }

        internal string SavePicture(string v1, object imageData, string v2)
        {
            throw new NotImplementedException();
        }
    }
}


