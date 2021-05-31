using System;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Threading.Tasks;
using ComputerGraphics.Converters.Ppm;
using ComputerGraphics.Converters.Sdk.Model;
using ComputerGraphics.Ioc.Framework;
using ComputerGraphics.ObjLoader;
using ComputerGraphics.RayTracing.Core.Entities;
using ComputerGraphics.RayTracing.Core.Entities.Materials;
using ComputerGraphics.RayTracing.Core.Entities.SceneObjects;
using ComputerGraphics.RayTracing.Core.Entities.Textures;
using ComputerGraphics.RayTracing.Core.Interfaces;
using ComputerGraphics.RayTracing.Entities.Entities;
using Camera = ComputerGraphics.RayTracing.Core.Entities.Cameras.Camera;
using Scene = ComputerGraphics.RayTracing.Core.Entities.Scene;
using LambertReflectionMaterial = ComputerGraphics.RayTracing.Core.Entities.Materials.LambertReflectionMaterial;

namespace ComputerGraphics.ConsoleApp
{
    public static class Program
    {
        private static readonly Assembly[] ModuleAssemblies =
        {
            typeof(RayTracing.Implementation.RayTracingImplementationProvider).Assembly,
            typeof(IObjLoader).Assembly,
            typeof(Program).Assembly,
            typeof(ComputerGraphics.PluginLoader.DependencyInjection).Assembly,
        };

        public static async Task Main(string[] args)
        {
            var container = new Container(ModuleAssemblies);
            var serviceCollection = container.Build();
            var rayTracer = serviceCollection.GetService<IRayTracer>();
            // var cowLoader = new ObjLoader.ObjLoader();
            // var cow = cowLoader.Load("cowfast.obj");
            // var sceneObject = new Mesh(cow.Faces.Select(_ => new Triangle(_.A, _.B, _.C)))
            // {
            // };
            // sceneObject.Id = 2;

            // sceneObject.Material = new DielectricMaterial()
            // {
            // RefractionIndex = .4f
            // };
            // sceneObject.Material = new NormalMaterial();
            // sceneObject.Material = new SolidColorMaterial() {Color = new (0, 0, 1)};
            var result = rayTracer.Trace(new Scene()
            {
                Cameras = ArraySegment<Camera>.Empty,
                SceneObjects = new SceneObject[]
                {
                    // new Box()
                    // {
                    // Material = new LambertReflectionMaterial()
                    // {
                    // Color = new Vector3(0f, 1f, 1f)
                    // },
                    // Transform = new Transform()
                    // {
                    // Position = new Vector3()
                    // {
                    // X = 1,
                    // Y = 1,
                    // Z = 3
                    // },

                    // },
                    // Size = new Vector3(1f, 1f, 1f)
                    // },
                    // new Sphere()
                    // {
                    // Material = new DiffuseLightMaterial()
                    // {
                    // Color = new Vector3(1f, 1f, 1f)
                    // },
                    // Radius = 100f,
                    // Transform = new Transform()
                    // {
                    // Position = new Vector3(0, 0, -5),
                    // Rotation = Vector3.Zero,
                    // Scale = Vector3.One
                    // }
                    // },
                    // sceneObject,
                    // new Triangle(new(0, 0, 2), new(1, .5f, 2), new(0, 1, 2))
                    // {
                    // Material = new LambertReflectionMaterial()
                    // {
                    // Albedo = new SolidColor(new (.6f, .7f, .2f))
                    // },
                    // },
                    // new Pyramid(2, 1, new Transform()
                    // {
                    //     Position = new Vector3(0,0, 2),
                    //     Rotation = Vector3.Zero,
                    //     Scale = new Vector3(1, 1, 1)
                    // })
                    // {
                    // Material = new SolidColorMaterial()
                    // {
                    // Color = new Vector3(1.0f, 0f, 0f)
                    // },
                    // },
                    new Sphere()
                    {
                        Material = new LambertReflectionMaterial()
                        {
                            Albedo = new SolidColor(new(.9f, .3f, .3f))
                        },
                        Radius = 1f,
                        Transform = new Transform()
                        {
                            Position = new(-3.5f, 0, 5),
                            Rotation = Vector3.Zero,
                            Scale = Vector3.One
                        }
                    },
                    new Sphere()
                    {
                        Material = new MetalMaterial()
                        {
                            Color = new(1, 1, 0),
                            Fuzz = 0
                        },
                        Radius = 1f,
                        Transform = new Transform()
                        {
                            Position = new Vector3(3, 0, 5),
                            Scale = Vector3.One
                        }
                    },

                    new Sphere()
                    {
                        Material = new DiffuseLightMaterial()
                        {
                            Color = new Vector3(30, 30, 30)
                        },
                        Radius = 1f,
                        Transform = new Transform()
                        {
                            Position = new Vector3(2, 8, 10),
                        }
                    },
                    // new Sphere()
                    // {
                    //     Material = new DielectricMaterial()
                    //     {
                    //         RefractionIndex = 1.5f,
                    //     },
                    //     Radius = 1f,
                    //     Transform = new Transform()
                    //     {
                    //         Position = new Vector3(0, 0, 5)
                    //     }
                    // },
                    new Disk()
                    {
                        Radius = 10,
                        Transform = new Transform()
                        {
                            Position = new Vector3(0, 0, -1)
                        },
                        Material = new LambertReflectionMaterial()
                        {
                            Albedo = new SolidColor(new(.3f, .3f, .3f))
                        }
                    }
                }
            });
            var rgbs = ConvertToRgb(result);
            var writer = new PpmEncoder();
            var encoded = writer.Encode(rgbs);
            await File.WriteAllBytesAsync("result.ppm", encoded);
        }


        private static RgbColor[,] ConvertToRgb(Vector3[,] traced)
        {
            int width = traced.GetLength(0);
            int height = traced.GetLength(1);
            var result = new RgbColor[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var current = traced[i, j];
                    result[i, j] = new RgbColor()
                    {
                        R = ToRgbComponent(current.X),
                        G = ToRgbComponent(current.Y),
                        B = ToRgbComponent(current.Z)
                    };
                }
            }

            return result;
        }

        private static byte ToRgbComponent(float v) => (byte) Math.Clamp(v * 255, 0, 255);
    }
}