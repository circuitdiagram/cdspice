// SpicePlugin.cs
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
using CircuitDiagram;
using System.ComponentModel.Composition;

namespace CDSpice
{
    /// <summary>
    /// Provides a writer for Circuit Diagram to export to the Spice netlist format.
    /// </summary>
    [Export(typeof(IPlugin))]
    public class SpicePlugin : IPlugin
    {
        /// <summary>
        /// The author of the plugin.
        /// </summary>
        public string Author { get { return "Circuit Diagram"; } }

        /// <summary>
        /// The GUID of the plugin
        /// </summary>
        public Guid GUID { get { return new Guid("A273E66E-2F25-4B78-9406-D4844A55843A"); } }

        /// <summary>
        /// The name of the plugin.
        /// </summary>
        public string Name { get { return "Spice Export"; } }

        /// <summary>
        /// Contains all plugin parts exposed by this plugin.
        /// </summary>
        public IList<IPluginPart> PluginParts { get; private set; }

        /// <summary>
        /// The version of the plugin.
        /// </summary>
        public string Version { get { return "1.0"; } }

        /// <summary>
        /// Initializes the plugin, loading the PluginParts.
        /// </summary>
        public SpicePlugin()
        {
            PluginParts = new List<IPluginPart>();
            PluginParts.Add(new SpiceWriter());
        }
    }
}
