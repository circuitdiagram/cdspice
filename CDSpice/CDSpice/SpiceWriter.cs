// SpiceWriter.cs
//
// Circuit Diagram http://www.circuit-diagram.org/
//
// Copyright (C) 2013 Circuit Diagram
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using CircuitDiagram.IO;
using System.IO;

namespace CDSpice
{
    /// <summary>
    /// Writes to the Spice netlist format.
    /// </summary>
    [Export(typeof(IDocumentWriter))]
    public class SpiceWriter : IElementDocumentWriter
    {
        /// <summary>
        /// The name of this plugin part.
        /// </summary>
        public string PluginPartName { get { return "Spice Netlist Exporter"; } }

        /// <summary>
        /// The file type name of the file to write - displayed in the "Export" dialog.
        /// </summary>
        public string FileTypeName { get { return "Text Files"; } }

        /// <summary>
        /// The file extension of the file to write.
        /// </summary>
        public string FileTypeExtension { get { return ".txt"; } }

        /// <summary>
        /// The document to write.
        /// </summary>
        public IODocument Document { get; set; }

        /// <summary>
        /// Called after the constructor.
        /// </summary>
        public void Begin()
        {
            // Do nothing
        }

        /// <summary>
        /// Called after the document has been written.
        /// </summary>
        public void End()
        {
            // Do nothing
        }

        /// <summary>
        /// Not currently used.
        /// </summary>
        public ISaveOptions Options { get; set; }

        /// <summary>
        /// Writes the document to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        public void Write(System.IO.Stream stream)
        {
            // Create writer
            StringWriter writer = new StringWriter();

            // Write simulation name
            writer.WriteLine(Document.Metadata.Title);
            writer.WriteLine("*** Created with Circuit Diagram Spice Exporter ***");
            writer.WriteLine();

            // Write netlist
            writer.WriteLine("*** Netlist Description ***");

            // Loop through components, writing to netlist
            foreach (IOComponent component in Document.Components)
            {
                // Determine component collection
                if (component.Type.Collection == Constants.CommonComponentsNamespace)
                {
                    // Determine component type
                    if (component.Type.Item == "resistor")
                    {
                        // Get connection names
                        string connectionA = FormatConnectionName(component.Connections["a"]);
                        string connectionB = FormatConnectionName(component.Connections["b"]);

                        // Get resistance
                        object resistance = component.Properties.First(property => property.Key == "resistance").Value;

                        // Format: R{componentId} {connectionA} {connectionB} {resistance}
                        writer.WriteLine("R{0} {1} {2} {3}", component.ID, connectionA, connectionB, resistance);
                    }
                    else if (component.Type.Item == "capacitor")
                    {
                        // Get connection names
                        string connectionA = FormatConnectionName(component.Connections["a"]);
                        string connectionB = FormatConnectionName(component.Connections["b"]);

                        // Get resistance
                        object capacitance = component.Properties.First(property => property.Key == "capacitance").Value;

                        // Format: R{componentId} {connectionA} {connectionB} {resistance}
                        writer.WriteLine("C{0} {1} {2} {3}", component.ID, connectionA, connectionB, capacitance);
                    }
                    else if (component.Type.Item == "rail")
                    {
                        // Get connection names
                        string connectionCom = FormatConnectionName(component.Connections["com"]);

                        // Get resistance
                        object voltage = component.Properties.First(property => property.Key == "voltage").Value;

                        // Format: R{componentId} {connectionA} {connectionB} {resistance}
                        writer.WriteLine("V{0} {1} gnd {2}", component.ID, connectionCom, voltage);
                    }
                    else if (component.Type.Item == "ground")
                    {
                        // Get connection names
                        string connectionCom = FormatConnectionName(component.Connections["com"]);

                        // Format: R{componentId} {connectionA} {connectionB} {resistance}
                        writer.WriteLine("V{0} {1} gnd 0", component.ID, connectionCom);
                    }
                }
            }

            // Write to file
            string dataString = writer.ToString();
            stream.Write(Encoding.UTF8.GetBytes(dataString), 0, Encoding.UTF8.GetByteCount(dataString));
        }

        /// <summary>
        /// Converts a connection name into an allowed value.
        /// </summary>
        /// <param name="name">Original connection name.</param>
        /// <returns>Connection name permitted for a spice file.</returns>
        static string FormatConnectionName(string name)
        {
            // The connection name '0' is used for ground in netlists
            if (name == "0")
                return "0_0";
            else
                return name;
        }
    }
}
