﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Numerics;
using ComputerGraphics.ObjLoader.Models;

namespace ComputerGraphics.ObjLoader
{
    public class ObjLoader : IObjLoader
    {
        private List<Vector3> _vertexList;
        private List<Vector3> _normalsList;
        
        private List<Triangle> _resultNormals;
        private List<Triangle> _resultFaces;

        
        public Object3D Load(string filename)
        {
            var lines = File.ReadAllLines(filename);
            
            _vertexList = new List<Vector3>();
            _normalsList = new List<Vector3>();
            _resultNormals = new List<Triangle>();
            _resultFaces = new List<Triangle>();

            foreach (var line in lines)
            {
                var parsedLine = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                if (parsedLine.Length == 0) continue;

                switch (parsedLine[0])
                {
                    case "v":
                        ReadVertex(parsedLine);
                        break;
                    case "vn":
                        ReadNormal(parsedLine);
                        break;
                    case "f":
                        ReadFace(parsedLine);
                        break;
                }
            }

            var resultObject = new Object3D();

            resultObject.Faces = _resultFaces;

            return resultObject;
        }

        private void ReadVertex(string[] line)
        {
            var vector = new Vector3();
            vector.X = float.Parse(line[1], CultureInfo.InvariantCulture.NumberFormat);
            vector.Y = float.Parse(line[2], CultureInfo.InvariantCulture.NumberFormat);
            vector.Z = float.Parse(line[3], CultureInfo.InvariantCulture.NumberFormat);

            _vertexList.Add(vector);
        }

        private void ReadNormal(string[] line)
        {
            var vector = new Vector3();
            vector.X = float.Parse(line[1], CultureInfo.InvariantCulture.NumberFormat);
            vector.Y = float.Parse(line[2], CultureInfo.InvariantCulture.NumberFormat);
            vector.Z = float.Parse(line[3], CultureInfo.InvariantCulture.NumberFormat);

            _normalsList.Add(Vector3.Normalize(vector));
        }

        private void ReadFace(string[] line)
        {
            var vertexIndexList = new List<int>();
            var normalsIndexList = new List<int>();
            
            for (var i = 1; i < line.Length; i++)
            {
                var item = line[i].Split("/");
                
                vertexIndexList.Add(Convert.ToInt32(item[0]));
                if (item.Length == 3)
                {
                    normalsIndexList.Add(Convert.ToInt32(item[2]));
                }
            }

            for (var i = 0; i < vertexIndexList.Count - 2; i++)
            {
                var vertexTriangle = new Triangle
                {
                    A = _vertexList[vertexIndexList[0] - 1],
                    B = _vertexList[vertexIndexList[i + 1] - 1],
                    C = _vertexList[vertexIndexList[i + 2] - 1]
                };

                if (normalsIndexList.Count == 3)
                {
                    vertexTriangle.NormalA = _normalsList[normalsIndexList[0] - 1];
                    vertexTriangle.NormalB = _normalsList[normalsIndexList[i + 1] - 1];
                    vertexTriangle.NormalC = _normalsList[normalsIndexList[i + 2] - 1];
                }
                
                _resultFaces.Add(vertexTriangle);
            }
        }
    }
}