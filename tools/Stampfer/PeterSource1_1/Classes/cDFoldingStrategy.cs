/**************************************************************************************
    Stampfer - Gothic Script Editor
    Copyright (C) 2008, 2009 Jpmon1, Alexander "Sumpfkrautjunkie" Ruppert

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
using System.Windows.Forms;
using System.Text.RegularExpressions;
namespace Peter
{
    class cDFoldingStrategy : IFoldingStrategy
    {
        /*Regex ro = new Regex(@"/\*");
        Regex rc = new Regex(@"\/");*/
        //Regex rc = new Regex(@"\*/");
        Regex lbrace = new Regex(@"{");
        Regex rbrace = new Regex(@"}");

        


        public List<FoldMarker> GenerateFoldMarkers(
            IDocument document, string fileName, object parseInformation)
        {
            List<FoldMarker> markers = new List<FoldMarker>();
            Stack<FoldStartMarker> startMarkers = new Stack<FoldStartMarker>();
            try
            {
                
                //MatchCollection mo = ro.Matches(document.TextContent);
                //MatchCollection mc = rc.Matches(document.TextContent);
                
                List<int> MatchListO=new List<int>();
                List<int> MatchListC = new List<int>();
                string text;
                string comment;
                int temp = 0;
                bool auf = false;
                for (int i = 0; i < document.TotalNumberOfLines; i++)
                {
                    text = document.GetText(document.GetLineSegment(i));

                    if (!auf &&(text.Contains("/*")))                       
                    {
                        temp = text.IndexOf("/*");
                        if ((text.Contains("//"))
                        && (text.IndexOf("//") < temp))
                        {
                        }
                        else
                        {

                            auf = true;
                            MatchListO.Add(temp + document.GetLineSegment(i).Offset);
                        }

                    }
                    if (auf && (text.Contains("*/")))
                    {
                        temp = text.IndexOf("*/");
                        if ((temp <= 0 || text[temp - 1] != '/'))
                        {
                            auf = false;
                            MatchListC.Add(temp + document.GetLineSegment(i).Offset);
                        }
                    }
                }
                
                //MessageBox.Show(MatchListO.Count.ToString() + " " + MatchListC.Count.ToString());
                /*int k = 0;
                while (k<mo.Count)
                {
                    if ((mo[k].Index > 0)&&(document.TextContent[mo[k].Index-1] !="/"))
                    {
                       
                    }
                    k++;
                }*/
                // Create foldmarkers for the whole document, enumerate through every line.
                for (int i = 0; i < document.TotalNumberOfLines; i++)
                {
                    // Get the text of current line.
                    text = document.GetText(document.GetLineSegment(i));
                    comment = "";

                    MatchCollection ml = lbrace.Matches(text);
                    foreach (Match m in ml)
                    {
                        if (!IsInComment(document, i, "{", MatchListO, MatchListC , m))
                        {
                            
                            startMarkers.Push(
                                CreateStartMarker(document, i, "{", "",m.Index));
                        }
                    }
                    


                    if (text.Contains("/*")) // Look for method starts
                    {
                        temp = text.IndexOf("/*");
                        comment = text.Substring(temp + 2);
                        if (!((text.Contains("//"))
                            && (text.IndexOf("//") < temp)))
                        {
                        
                            startMarkers.Push(
                                CreateStartMarker(document, i, "/*", comment, temp));
                        }
                    }
                    if (text.Contains("//:+")) // Look for method starts
                    {
                        temp = text.IndexOf("//:+");
                        comment = text.Substring(temp + 4);

                        startMarkers.Push(
                            CreateStartMarker(document, i, "//:+", comment, temp));

                    }
                    if (text.Contains("//:-")) // Look for method starts
                    {

                        FoldStartMarker startMarker = startMarkers.Pop();
                        FoldMarker foldMarker = CreateFoldMarker(document, i, startMarker, text.IndexOf("//:-"), "//:-", false);
                        if (foldMarker != null)
                        {
                            markers.Add(foldMarker);
                        }

                    }
                    if (text.Contains("*/")) // Look for method starts
                    {
                        temp = text.IndexOf("*/");
                        if (!((text.Contains("//"))
                            && (text.IndexOf("//") < temp)))
                        {

                            if ((text.IndexOf("*/") <= 0 || text[temp - 1] != '/'))
                            {
                                
                                FoldStartMarker startMarker = startMarkers.Pop();
                                FoldMarker foldMarker = CreateFoldMarker(document, i, startMarker, temp, "*/", false);
                                if (foldMarker != null)
                                {
                                    markers.Add(foldMarker);
                                }
                            }
                        }
                        

                    }

                    
                        MatchCollection mr = rbrace.Matches(text);
                        foreach (Match m in mr)
                        {
                            if (!IsInComment(document, i, "}", MatchListO, MatchListC, m))
                            {

                                FoldStartMarker startMarker = startMarkers.Pop();
                                FoldMarker foldMarker = CreateFoldMarker(document, i, startMarker, m.Index, "}", true);
                                if (foldMarker != null)
                                {
                                    markers.Add(foldMarker);
                                }
                            }
                        }
                        
                       // markers.Add(new FoldMarker(document, start, document.GetLineSegment(start).Length, i, 0, FoldType.TypeBody));
                    
                }
                
            }
            catch
            {
               //return (List<FoldMarker>)document.FoldingManager.FoldMarker;
            }
            /*foreach (FoldMarker f in markers)
            {
                MessageBox.Show(f.StartLine.ToString()+"  "+f.Offset.ToString());
            }*/
            return markers;
        }

        private bool IsInComment(IDocument document, int i, string s, List<int> mo, List<int> mc, Match m)
        {
            string text = document.GetText(document.GetLineSegment(i));
            
            if (text.Contains("//"))
            {
                if (text.IndexOf("//") < m.Index)
                {
                    return true;
                }
            }
            
            //MessageBox.Show(ro.ToString()+mo.Count.ToString()+mc.Count.ToString());
            int l = m.Index + document.GetLineSegment(i).Offset;
            //MessageBox.Show(mo[0].Index.ToString());
            int k=0;
            
            while (k< mc.Count)
            {
                
                if (mc[k] > l)
                {

                    
                    if (mo[k] < l)
                    {
                        
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                
                k++;
            }




            return false;
        }

        private FoldStartMarker CreateStartMarker(IDocument document,int i,string s, string t, int pos)
        {
            //string text = document.GetText(document.GetLineSegment(i));
            FoldStartMarker startMarker = new FoldStartMarker(
                pos /*+ s.Length*/, i,
                t, "");
            return startMarker;
        }

        private FoldMarker CreateFoldMarker(IDocument document, int i,
            FoldStartMarker startMarker, int pos, string s2, bool b)
        {
            string text = document.GetText(document.GetLineSegment(i));
            int endLineNumber = i;
            FoldMarker marker = null;
            if (endLineNumber > startMarker.LineNumber)
            {
                marker = new FoldMarker(document, startMarker.LineNumber,
                    startMarker.Column, endLineNumber, text.Length, b ? FoldType.TypeBody : FoldType.Region, startMarker.FoldText);
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
                    return (_name.Length>0?(_name):"...");
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
