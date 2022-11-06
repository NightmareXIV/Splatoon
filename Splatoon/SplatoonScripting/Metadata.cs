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
        /// Name of a script
        /// </summary>
        public string Name { get; }

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

        public Metadata(string name, uint version, string? author, string? description, string? website)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Version = version;
            Author = author;
            Description = description;
            Website = website;
        }

        public Metadata(string name, uint version, string? author, string? description)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Version = version;
            Author = author;
            Description = description;
        }

        public Metadata(string name, uint version, string? author)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Version = version;
            Author = author;
        }

        public Metadata(string name, uint version)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Version = version;
        }

        public Metadata(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
