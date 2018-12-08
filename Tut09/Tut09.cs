using System;
using System.Collections.Generic;
using System.Linq;
using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Math.Core;
using Fusee.Serialization;
using Fusee.Xene;
using static System.Math;
using static Fusee.Engine.Core.Input;
using static Fusee.Engine.Core.Time;


namespace Fusee.Tutorial.Core
{
    public class Tut09 : RenderCanvas
    {
        private SceneContainer _scene;
        private SceneRenderer _sceneRenderer;
        private float _cameraAngle = 0;
        private TransformComponent _baseTransform;
        private TransformComponent _bodyTransform;
        private TransformComponent _upperArmTransform;
        private TransformComponent _foreArmTransform;
        private TransformComponent _grip1Transform;
        private TransformComponent _grip2Transform;
        
        private bool _gripTrigger = false;



        SceneContainer CreateScene()
        {
            // Initialize transform components that need to be changed inside "RenderAFrame"
            _baseTransform = new TransformComponent
            {
                Rotation = new float3(0, 0.2f, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 0, 0)
            };

             _bodyTransform = new TransformComponent
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 6, 0)
            };

             _upperArmTransform = new TransformComponent
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(2, 4, 0)
            };

             _foreArmTransform = new TransformComponent
            {
                Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(2, 8, 0)
            };
            _grip1Transform = new TransformComponent
            {
               Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 8, 0.6f)
            };

            _grip2Transform = new TransformComponent
            {
               Rotation = new float3(0, 0, 0),
                Scale = new float3(1, 1, 1),
                Translation = new float3(0, 8, -0.6f)
            };


            // Setup the scene graph
            return new SceneContainer
            {
                Children = new List<SceneNodeContainer>
                {
                    // GREY BASE
                    new SceneNodeContainer
                    {
                        Components = new List<SceneComponentContainer>
                        {
                            // TRANSFROM COMPONENT
                            _baseTransform,

                            // MATERIAL COMPONENT
                        new MaterialComponent
                        {
                            Diffuse = new MatChannelContainer { Color = new float3(0.7f, 0.7f, 0.7f) },
                            Specular = new SpecularChannelContainer { Color = new float3(1, 1, 1), Shininess = 5 }
                        },

                            // MESH COMPONENT
                            SimpleMeshes.CreateCuboid(new float3(10, 2, 10))
                        }
                    },

                    // RED BODY
                    new SceneNodeContainer
                    {
                        Components = new List<SceneComponentContainer>
                        {
                            _bodyTransform,
                            new ShaderEffectComponent
                            {
                                Effect = SimpleMeshes.MakeShaderEffect(new float3(1, 0, 0), new float3(0.7f, 0.7f, 0.7f), 5)
                            },

                            SimpleMeshes.CreateCuboid(new float3(2, 10, 2))
                        },

                        Children = new List<SceneNodeContainer>
                        {
                            new SceneNodeContainer
                            {
                                Components = new List<SceneComponentContainer>
                                {
                                    _upperArmTransform,
                                },
                                Children = new List<SceneNodeContainer>
                                {
                                    new SceneNodeContainer
                                    {
                                        Components = new List<SceneComponentContainer>
                                        {
                                            new TransformComponent
                                            {
                                                Rotation = new float3(0, 0, 0),
                                                Scale = new float3(1, 1, 1),
                                                Translation = new float3(0, 4, 0)
                                            },
                                           
                                            new ShaderEffectComponent
                                            {
                                                Effect = SimpleMeshes.MakeShaderEffect(new float3(0, 1, 0), new float3(0.7f, 0.7f, 0.7f), 5)
                                            },
                                            SimpleMeshes.CreateCuboid(new float3(2, 10, 2))
                                        }
                                    },
                                    new SceneNodeContainer
                                    {
                                        Components = new List<SceneComponentContainer>
                                        {
                                            _foreArmTransform,
                                        },
                                        Children = new List<SceneNodeContainer>
                                        {
                                            new SceneNodeContainer
                                            {
                                                Components = new List<SceneComponentContainer>
                                                {
                                                    new TransformComponent
                                                    {
                                                        Rotation = new float3(0, 0, 0),
                                                        Scale = new float3(1, 1, 1),
                                                        Translation = new float3(-4, 4, 0)
                                                    },
                                                    new ShaderEffectComponent
                                                    {
                                                        Effect = SimpleMeshes.MakeShaderEffect(new float3(0, 0, 1), new float3(0.7f, 0.7f, 0.7f), 5)
                                                    },
                                                    SimpleMeshes.CreateCuboid(new float3(2, 10, 2))
                                                }
                                            },
                                            new SceneNodeContainer
                                            {
                                                Components =  new List<SceneComponentContainer>
                                                {
                                                    _grip1Transform,
                                                },
                                                Children = new List<SceneNodeContainer>
                                                {
                                                    new SceneNodeContainer
                                                    {
                                                        Components = new List<SceneComponentContainer>
                                                        {
                                                            new TransformComponent
                                                            {
                                                                Rotation = new float3(0, 0, 0),
                                                                Scale = new float3(1, 1, 1),
                                                                Translation = new float3(-4, 2, 0)
                                                            },
                                                            new ShaderEffectComponent
                                                            {
                                                                Effect = SimpleMeshes.MakeShaderEffect(new float3(0.7f, 0, 0.5f), new float3(0.7f, 0.7f, 0.7f), 5)
                                                            },

                                                            SimpleMeshes.CreateCuboid(new float3(1.5f, 3.5f, 0.5f))
                                                        }
                                                    }
                                                }
                                            },
                                            new SceneNodeContainer
                                            {
                                                Components =  new List<SceneComponentContainer>
                                                {
                                                    _grip2Transform,
                                                },
                                                Children = new List<SceneNodeContainer>
                                                {
                                                    new SceneNodeContainer
                                                    {
                                                        Components = new List<SceneComponentContainer>
                                                        {
                                                            new TransformComponent
                                                            {
                                                                Rotation = new float3(0, 0, 0),
                                                                Scale = new float3(1, 1, 1),
                                                                Translation = new float3(-4, 2, 0)
                                                            },
                                                            new ShaderEffectComponent
                                                            {
                                                                Effect = SimpleMeshes.MakeShaderEffect(new float3(0.7f, 0, 0.5f), new float3(0.7f, 0.7f, 0.7f), 5)
                                                            },

                                                            SimpleMeshes.CreateCuboid(new float3(1.5f, 3.5f, 0.5f))
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            },
                        }
                    }
                }
            };
        }


        
        // Init is called on startup. 
        public override void Init()
        {
            
            // Set the clear color for the backbuffer to white (100% intensity in all color channels R, G, B, A).
            RC.ClearColor = new float4(0.7f, 1, 0.5f,0.5f);

            _scene = CreateScene();

            // Create a scene renderer holding the scene above
            _sceneRenderer = new SceneRenderer(_scene);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            float bodyRot = _bodyTransform.Rotation.y;
            bodyRot += 0.01f * Keyboard.LeftRightAxis;
            _bodyTransform.Rotation = new float3(0, bodyRot, 0);

            float upperArmRot = _upperArmTransform.Rotation.x;
            upperArmRot += 0.01f * Keyboard.UpDownAxis;
            _upperArmTransform.Rotation = new float3(upperArmRot, 0, 0);

            float foreArmRot = _foreArmTransform.Rotation.x;
            foreArmRot += 0.01f * Keyboard.WSAxis;
            _foreArmTransform.Rotation = new float3(foreArmRot, 0, 0);
           

           float gripRot = _grip1Transform.Rotation.x;
            if(Keyboard.IsKeyDown(KeyCodes.Space)){
                if(_gripTrigger == true){
                    _gripTrigger = false;
                } else {
                    _gripTrigger = true;
                }
            }

            if(_gripTrigger == true){
                if(gripRot <= 0.5f){
                    gripRot += 0.01f;
                } else {
                    gripRot = 0.5f;
                }
            } else {
                if(gripRot >= -0.1f){
                    gripRot -= 0.01f;
                } else {
                    gripRot = -0.1f;
                }
            }
            _grip1Transform.Rotation = new float3(gripRot, 0.0f, 0.0f);
            _grip2Transform.Rotation = new float3(-gripRot, 0.0f, 0.0f);

            if(Mouse.LeftButton == true){
                _cameraAngle += Mouse.Velocity.x * 0.01f * DeltaTime;
            }


            // Setup the camera 
            RC.View = float4x4.CreateTranslation(0, -10, 50) * float4x4.CreateRotationY(_cameraAngle);

            // Render the scene on the current render context
            _sceneRenderer.Render(RC);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered farame) on the front buffer.
            Present();
        }


        // Is called when the window was resized
        public override void Resize()
        {
            // Set the new rendering area to the entire new windows size
            RC.Viewport(0, 0, Width, Height);

            // Create a new projection matrix generating undistorted images on the new aspect ratio.
            var aspectRatio = Width / (float)Height;

            // 0.25*PI Rad -> 45° Opening angle along the vertical direction. Horizontal opening angle is calculated based on the aspect ratio
            // Front clipping happens at 1 (Objects nearer than 1 world unit get clipped)
            // Back clipping happens at 2000 (Anything further away from the camera than 2000 world units gets clipped, polygons will be cut)
            var projection = float4x4.CreatePerspectiveFieldOfView(M.PiOver4, aspectRatio, 1, 20000);
            RC.Projection = projection;
        }
    }
}