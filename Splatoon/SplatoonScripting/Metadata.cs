using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable enable
namespace Splatoon.SplatoonScripting
{
    public class Metadata
    {
        /// <summary>
        /// Optional single digit version of a script, will be displayed in the list of scripts.
        /// </summary>
        public uint Version { get; }

        /// <summary>
        /// Optional author of a script, will be displayed in the list of scripts.
        /// </summary>
        public string? Author { get; }

        /// <summary>
        /// Optional description of a script, will be displayed in the list of scripts.
        /// </summary>
        public string? Description { get; }

        /// <summary>
        /// Optional website of script's origin.
        /// </summary>
        public string? Website { get; }

        /// <summary>
        /// URL for auto-update. Can be direct to the file or to the folder containing the script. Remote file name is ignored.
        /// </summary>
        public string? UpdateURL { get; set; }

        public Metadata(uint version, string? author, string? description, string? website)
        {
            Version = version;
            Author = author;
            Description = description;
            Website = website;
        }

        public Metadata(uint version, string? author, string? description)
        {
            Version = version;
            Author = author;
            Description = description;
        }

        public Metadata(uint version, string? author)
        {
            Version = version;
            Author = author;
        }

        public Metadata(uint version)
        {
            Version = version;
        }

        public Metadata()
        {
        }
    }
}
