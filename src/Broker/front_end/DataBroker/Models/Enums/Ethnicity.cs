using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataBroker.Models.Enums
{
    [Flags]
    public enum Ethnicity
    {
        [Display(Name = "Others")]
        Others = 1,

        [Display(Name = "Asian")]
        Asian = 2,

        [Display(Name = "Black/African")]
        BlackAfrican = 4,

        [Display(Name = "Caucasian")]
        Caucasian = 8,

        [Display(Name = "Hispanic/Latin")]
        HispanicLatin = 16,

        [Display(Name = "Native American")]
        NativeAmerican = 32,

        [Display(Name = "Pacific Islander")]
        PacificIslander = 64,
    }
}
