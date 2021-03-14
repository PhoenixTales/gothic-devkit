/**************************************************************************************
    Stampfer - Gothic Script Editor
    Copyright (C) 2008  Jpmon1

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
**************************************************************************************/
using System;
using System.Collections.Generic;
using ICSharpCode.TextEditor.Document;
using System.Xml;
using System.IO;

namespace Peter
{
    class cXmlFoldingStrategy : IFoldingStrategy
    {
        public List<FoldMarker> GenerateFoldMarkers(
            IDocument document, string fileName, object parseInformation)
        {
            List<FoldMarker> markers = new List<FoldMarker>();
            Stack<FoldStartMarker> startMarkers = new Stack<FoldStartMarker>();
            try
            {
                using (XmlTextReader xmlReader = new XmlTextReader(
                    new StringReader(document.TextContent)))
                {
                    while (xmlReader.Read())
                    {
                        if (xmlReader.NodeType == XmlNodeType.Element &&
                            xmlReader.IsEmptyElement == false)
                        {
                            startMarkers.Push(
                                CreateStartMarker(xmlReader));
                        }
                        else if (xmlReader.NodeType == XmlNodeType.EndElement)
                        {
                            FoldStartMarker startMarker = startMarkers.Pop();
                            FoldMarker foldMarker = CreateFoldMarker(document, xmlReader, startMarker);
                            if (foldMarker != null)
                            {
                                markers.Add(foldMarker);
                            }
                        }
                        else if (xmlReader.NodeType == XmlNodeType.Comment)
                        {
                            FoldMarker foldMarker = CreateCommentFoldMarker(document, xmlReader);
                            if (foldMarker != null)
                            {
                                markers.Add(foldMarker);
                            }
                        }
                    }
                }
            }
            catch
            {
                return (List<FoldMarker>)document.FoldingManager.FoldMarker;
            }
            return markers;
        }

        private FoldStartMarker CreateStartMarker(XmlTextReader xmlReader)
        {
            FoldStartMarker startMarker = new FoldStartMarker(
                xmlReader.LinePosition - 2, xmlReader.LineNumber - 1,
                xmlReader.LocalName, xmlReader.Prefix);
            return startMarker;
        }

        private FoldMarker CreateFoldMarker(IDocument document, XmlTextReader reader,
            FoldStartMarker startMarker)
        {
            int endLineNumber = reader.LineNumber - 1;
            FoldMarker marker = null;
            if (endLineNumber > startMarker.LineNumber)
            {
                marker = new FoldMarker(document, startMarker.LineNumber,
                    startMarker.Column, endLineNumber, reader.LinePosition +
                    startMarker.QualifiedName.Length, FoldType.TypeBody, startMarker.FoldText);
            }
            return marker;
        }

        private FoldMarker CreateCommentFoldMarker(IDocument document, XmlTextReader xmlReader)
        {
            FoldMarker marker = null;
            string comment = xmlReader.Value;
            if (String.IsNullOrEmpty(comment) == false)
            {
                string[] lines = comment.Replace(Environment.NewLine, "\n").Split('\n');
                if (lines.Length > 1)
                {
                    int startLineNumber = xmlReader.LineNumber - 1;
                    int startColumn = xmlReader.LinePosition - 5;
                    int endLine = startLineNumber + lines.Length - 1;
                    int endColumn = lines[lines.Length - 1].Length + 3;
                    string foldText = "...";
                    marker = new FoldMarker(document, startLineNumber, startColumn,
                        endLine, endColumn, FoldType.TypeBody, foldText);
                }
            }
            return marker;
        }

        struct FoldStartMarker
        {
            int _column;
            int _lineNumber;
            string _name;
            string _prefix;

            public int Column
            {
                get { return _column; }
            }

            public int LineNumber
            {
                get { return _lineNumber; }
            }

            public string FoldText
            {
                get
                {
                    return String.Concat(
                        "<", QualifiedName, ">", "...");
                }
            }

            public string QualifiedName
            {
                get
                {
                    return String.IsNullOrEmpty(_prefix) ?
                        _name : _prefix + ":" + _name;
                }
            }

            public FoldStartMarker(int column, int lineNumber,
                string name, string prefix)
            {
                _column = column;
                _lineNumber = lineNumber;
                _name = name;
                _prefix = prefix;
            }
        }
    }

}
