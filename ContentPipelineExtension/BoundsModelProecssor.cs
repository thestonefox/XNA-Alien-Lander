using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System.ComponentModel;
using System.Drawing.Design;

// TODO: replace these with the processor input and output types.
using TInput = System.String;
using TOutput = System.String;

namespace ContentPipelineExtension
{
    /// <summary>
    /// Model Data Slots
    /// </summary>
    public enum ModelDataSlot
    {
        /// <summary>
        /// Bounding Box Data
        /// </summary>
        BoundingBoxs,
        /// <summary>
        /// Bounding Sphere data.
        /// </summary>
        BoundingSpheres
    }
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentProcessor attribute to specify the correct
    /// display name for this processor.
    /// </summary>
    [ContentProcessor(DisplayName = "Bounds Model Processor")]
    public class BoundsModelProcessor : ModelProcessor
    {
        List<BoundingBox> boxs = new List<BoundingBox>();
        List<BoundingSphere> spheres = new List<BoundingSphere>();        

        ContentProcessorContext context;

        bool enableLogging = false;

        /// <summary>
        /// Enable Logging
        /// </summary>
        [DefaultValue(false)]
        [Description("Set to true if you want logging on"), TypeConverter(typeof(BooleanConverter)), Editor(typeof(UITypeEditor), typeof(UITypeEditor))]
        [Category("Blacksun")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DisplayName("Enable Logging")]
        public bool EnableLogging
        {
            get { return enableLogging; }
            set
            {
                enableLogging = value;
            }
        }

        /// <summary>
        /// Process method.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override ModelContent Process(NodeContent input, ContentProcessorContext context)
        {
            string modelName = input.Identity.SourceFilename.Substring(input.Identity.SourceFilename.LastIndexOf("\\") + 1);
            if (EnableLogging)
                LogWriter.WriteToLog(string.Format("Process started for {0}", modelName));

            this.context = context;

            Dictionary<string, object> ModelData = new Dictionary<string, object>();

            ModelContent baseModel = base.Process(input, context);

            GenerateData(input);

            ModelData.Add(ModelDataSlot.BoundingBoxs.ToString(), boxs);
            ModelData.Add(ModelDataSlot.BoundingSpheres.ToString(), spheres);

            baseModel.Tag = ModelData;

            if (EnableLogging)
                LogWriter.WriteToLog(string.Format("Process completed for {0}", modelName));

            return baseModel;
        }

        private void GenerateData(NodeContent node)
        {
            MeshContent mesh = node as MeshContent;

            if (mesh != null)
            {
                MeshHelper.OptimizeForCache(mesh);

                // Look up the absolute transform of the mesh.
                Matrix absoluteTransform = mesh.AbsoluteTransform;

                int i = 0;

                // Loop over all the pieces of geometry in the mesh.
                foreach (GeometryContent geometry in mesh.Geometry)
                {
                    Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
                    Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

                    // Loop over all the indices in this piece of geometry.
                    // Every group of three indices represents one triangle.
                    List<Vector3> thisVerts = new List<Vector3>();
                    List<int> ind = new List<int>();
                    
                    Vector3 vertex = Vector3.Zero;
                    
                    foreach (int index in geometry.Indices)
                    {
                        // Look up the position of this vertex.
                        vertex = Vector3.Transform(geometry.Vertices.Positions[index], absoluteTransform);

                        // Store this data.
                        min = Vector3.Min(min, vertex);
                        max = Vector3.Max(max, vertex);
                                                
                        thisVerts.Add(vertex);
                        
                        ind.Add(i++);
                    }

                    if (EnableLogging)
                    {
                        LogWriter.WriteToLog(string.Format("BoundingBox created min = {0}, max = {1}", min, max));
                    }
                    boxs.Add(new BoundingBox(min, max));
                    spheres.Add(BoundingSphere.CreateFromBoundingBox(boxs[boxs.Count - 1]));                    
                }
            }

            // Recursively scan over the children of this node.
            foreach (NodeContent child in node.Children)
            {
                GenerateData(child);
            }
        }

    }
}