using C2108G1_Project_3.Controllers;
using C2108G1_Project_3.Data;
using C2108G1_Project_3.Models;
using C2108G1_Project_3.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Quartz;

namespace C2108G1_Project_3.JOB
{
    public class UpdateRoleJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            
           Console.WriteLine("JOB RUNNING");
            return Task.CompletedTask;
         
        }
    }
}
