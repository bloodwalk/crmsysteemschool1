using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PluralsightDemo
{
    public class PluralsightUser : IdentityUser
    {
        public PluralsightUser() {

           // Notities = new ICollection<Notitie>();
        }

        public string adress { get; set; }
        public string postcode { get; set; }
        public string woonplaats { get; set; }
        public string naam { get; set; }
        public bool IsPending { get; set; }
        // new data
        public DateTime deadline { get; set; }
        //public int NotititieId { get; set; }
        public ICollection<Notitie> Notities { get; set; }
        // public string somenewvariable { get; set; } = "test";
      //  public DbSet<pendingobject2> UserLogt { get; set; }

    }


    public class Notitie {

        
        public int Id { get; set; }

        [DataType(DataType.MultilineText)]
        public string Textarea { get; set; }

        public DateTime Datum { get; set; }
        public string onderwerp { get; set; }
        public PluralsightUser User { get; set; }
        public string UserId { get; set; }

    }


    //<textarea asp-for="Body" class="form-control"></textarea>// de html


    //public class pendingobject
    //{

    //    public int ID { get; set; }
    //    public string naam { get; set; }

    //    public string leeftijd { get; set; }
    //    public int anothervairalbe { get; set; }
        
    //}

    //public class pendingstudent
    //{
    //    [Key]
    //    public int ID2 { get; set; }       
    //    public string pendingnaam { get; set; }
    //    public string pendingadress { get; set; }
    //    public string pendingwoonplaats { get; set; }
    //    public string pendingpostcode { get; set; }

    //}
    

    //public class UserLog
    //{
    //    [Key]
    //    public Guid UserLogID { get; set; }

    //    public string IPAD { get; set; }
    //    public DateTime LoginDate { get; set; }
    //    public string UserId { get; set; }

    //    [ForeignKey("UserId")]
    //    public virtual ApplicationUser User { get; set; }
    //}

   



}