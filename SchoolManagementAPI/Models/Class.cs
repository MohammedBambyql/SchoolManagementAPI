﻿namespace SchoolManagementAPI.Models;
public class Class
{
    public int ClassId { get; set; }
    public string ClassName { get; set; }
    public int TeacherId { get; set; }

    public Teacher? Teacher { get; set; } 
}
