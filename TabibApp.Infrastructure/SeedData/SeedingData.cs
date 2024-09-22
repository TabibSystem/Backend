using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TabibApp.Infrastructure;

public class SeedingData
{
    public static void SeedGovernorates(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Governorate>().HasData(
            new Governorate { Id = 1, Name = "القاهرة" },
            new Governorate { Id = 2, Name = "الإسكندرية" },
            new Governorate { Id = 3, Name = "الجيزة" },
            new Governorate { Id = 4, Name = "الشرقية" },
            new Governorate { Id = 5, Name = "الدقهلية" },
            new Governorate { Id = 6, Name = "الغربية" },
            new Governorate { Id = 7, Name = "المنوفية" },
            new Governorate { Id = 8, Name = "الفيوم" },
            new Governorate { Id = 9, Name = "بني سويف" },
            new Governorate { Id = 10, Name = "السويس" },
            new Governorate { Id = 11, Name = "المنيا" },
            new Governorate { Id = 12, Name = "أسيوط" },
            new Governorate { Id = 13, Name = "قنا" },
            new Governorate { Id = 14, Name = "الأقصر" },
            new Governorate { Id = 15, Name = "أسوان" },
            new Governorate { Id = 16, Name = "بورسعيد" },
            new Governorate { Id = 17, Name = "دمياط" },
            new Governorate { Id = 18, Name = "مرسى مطروح" },
            new Governorate { Id = 19, Name = "شمال سيناء" },
            new Governorate { Id = 20, Name = "جنوب سيناء" }
        );
    }
     public static void SeedSpecializations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Specialization>().HasData(
                new Specialization { Id = 1, Name = "طب الأسرة" },              // Family Medicine
                new Specialization { Id = 2, Name = "الطب الباطني" },            // Internal Medicine
                new Specialization { Id = 3, Name = "جراحة العظام" },            // Orthopedic Surgery
                new Specialization { Id = 4, Name = "جراحة الأعصاب" },           // Neurosurgery
                new Specialization { Id = 5, Name = "طب الأطفال" },              // Pediatrics
                new Specialization { Id = 6, Name = "طب النساء والتوليد" },      // Obstetrics and Gynecology
                new Specialization { Id = 7, Name = "طب الأسنان" },              // Dentistry
                new Specialization { Id = 8, Name = "الأنف والأذن والحنجرة" },    // ENT (Otorhinolaryngology)
                new Specialization { Id = 9, Name = "الأشعة" },                  // Radiology
                new Specialization { Id = 10, Name = "الطب النفسي" },             // Psychiatry
                new Specialization { Id = 11, Name = "الجراحة العامة" },         // General Surgery
                new Specialization { Id = 12, Name = "أمراض القلب" },            // Cardiology
                new Specialization { Id = 13, Name = "أمراض الجهاز التنفسي" },   // Pulmonology
                new Specialization { Id = 14, Name = "أمراض الكلى" },            // Nephrology
                new Specialization { Id = 15, Name = "أمراض الدم" },             // Hematology
                new Specialization { Id = 16, Name = "الأمراض المعدية" },        // Infectious Diseases
                new Specialization { Id = 17, Name = "الطب الشرعي" },            // Forensic Medicine
                new Specialization { Id = 18, Name = "طب الطوارئ" },             // Emergency Medicine
                new Specialization { Id = 19, Name = "الأعصاب" },                // Neurology
                new Specialization { Id = 20, Name = "التخدير" },                // Anesthesiology
                new Specialization { Id = 21, Name = "طب الرياضة" },             // Sports Medicine
                new Specialization { Id = 22, Name = "الطب الوقائي" },           // Preventive Medicine
                new Specialization { Id = 23, Name = "الجراحة التجميلية" },     // Plastic Surgery
                new Specialization { Id = 24, Name = "الأمراض الجلدية" },        // Dermatology
                new Specialization { Id = 25, Name = "طب الشيخوخة" },            // Geriatrics
                new Specialization { Id = 26, Name = "طب الأوعية الدموية" },     // Vascular Medicine
                new Specialization { Id = 27, Name = "الأمراض النفسية" },        // Behavioral Health
                new Specialization { Id = 28, Name = "طب العيون" },              // Ophthalmology
                new Specialization { Id = 29, Name = "طب الأذن والحنجرة" }        // Otolaryngology
            );
            
        }

        public static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "Patient", NormalizedName = "PATIENT" },
                new IdentityRole { Id = "3", Name = "Doctor", NormalizedName = "DOCTOR" },
               new IdentityRole { Id = "4", Name = "Assistant", NormalizedName = "ASSISTANT" }
            );
        }
}