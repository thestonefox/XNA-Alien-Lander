using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace AlienGrab
{
    class Scene
    {
        public GameCamera Camera;
        public LightSource Light;
        public RenderTarget2D ShadowRenderTarget;

        public Scene(Game game)
        {
            Camera = new GameCamera(50.0f,
                                    new Vector3[] { new Vector3(1830.0f, -1100.0f, 250.0f), new Vector3(1830.0f, -1100.0f, 250.0f) },
                                    new Vector3[] { new Vector3(-600.0f, 1000.0f, 250.0f), new Vector3(600.0f, 1000.0f, 250.0f) },
                                    game.GraphicsDevice.Viewport.AspectRatio, 10.0f, 10000.0f,
                                    new Vector3(-600.0f, 1000.0f, 250.0f),
                                    new Vector3(1830.0f, -1100.0f, 250.0f));

            Light = new LightSource(new Vector3(-0.3f, 500.0f, 0.5f), Camera);

            ShadowRenderTarget = new RenderTarget2D(game.GraphicsDevice, 2048, 2048, false, SurfaceFormat.Single, DepthFormat.Depth24);
        }
    }
}
