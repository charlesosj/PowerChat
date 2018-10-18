using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PowerChat.DB
{


    public  class PowerChatDB : DbContext
    {
       
        public virtual DbSet<ChatLog> ChatLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.UseSqlServer(@"insert db connectionstirng");

        }
        public List<ChatLog> GetChatLogs(DateTime timeStamp )
        {
            List<ChatLog> ChatLogs;
           
                ChatLogs = this.ChatLogs
                   .Where(c => c.TimeStamp >=timeStamp)
                   .OrderBy(b => b.TimeStamp)
                   .ToList();

            return  ChatLogs;

        }
    }

    [Table("ChatLog")]
    public  class ChatLog
    {
        public int ID { get; set; }

        public DateTime TimeStamp { get; set; }

        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [StringLength(500)]
        public string Message { get; set; }


    }
}
