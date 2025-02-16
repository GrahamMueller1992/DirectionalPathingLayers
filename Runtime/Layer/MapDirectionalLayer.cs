﻿
//TODO
//Delegate callbacks
//Layer modified
namespace DirectionalPathingLayers
{
    public class MapDirectionalLayer
    {
        DirectionalLayer directionLayer;
        DirectionalLayer addedCountLayer;
        DirectionalNode defaultNode;

        public MapDirectionalLayer(int perSideWidth, int perSideLength, DirectionalNode defaultNode)
        {
            this.defaultNode = defaultNode;
            this.directionLayer = new DirectionalLayer(perSideWidth, perSideLength);
            this.directionLayer.Set(this.defaultNode);

            this.addedCountLayer = new DirectionalLayer(perSideWidth, perSideLength);

        }

        public DirectionalLayer DirectionLayer { get => this.directionLayer; }
        public DirectionalLayer AddedCountLayer { get => this.addedCountLayer; }


        /// <summary>
        /// Adds 'addingLayer' to this layer and modifies the cost. If a negative value 'addingLayer' is used, this will subtract the cost instead.
        /// 'addingLayer' is treated as directions that oppose 'defaultNode'.  
        /// If any point from addingLayer falls outside this layer's defined area, they are skipped.
        /// </summary>
        public void AddDirectionalLayerAtPoint(DirectionalLayer addingLayer, int centerX, int centerY)
        {
            if (addingLayer == null) { return; }

            //Get indexes to work with.
            int addingLayer_minX = centerX + this.directionLayer.GetSideWidth() - addingLayer.GetSideWidth();
            int addingLayer_minY = centerY + this.directionLayer.GetSideLength() - addingLayer.GetSideLength();
            int addingLayer_maxX = centerX + this.directionLayer.GetSideWidth() + addingLayer.GetSideWidth();
            int addingLayer_maxY = centerY + this.directionLayer.GetSideLength() + addingLayer.GetSideLength();

            for (int x = addingLayer_minX; x <= addingLayer_maxX; ++x)
            {
                for (int y = addingLayer_minY; y <= addingLayer_maxY; ++y)
                {
                    this.AddDirectionPointAtPoint(x, y, addingLayer.directionalNodes[x - addingLayer_minX, y - addingLayer_minY]);
                }
            }
        }

        /// <summary>
        /// Given index position, 'adds' the adding node if the points differ from the default node.  
        /// Supports negative addingNode for subtraction
        /// </summary>
        void AddDirectionPointAtPoint(int indexX, int indexY, DirectionalNode addingNode)
        {
            if (indexX >= 0 &&
                indexX < this.directionLayer.directionalNodes.GetLength(0) &&
                indexY >= 0 &&
                indexY < this.directionLayer.directionalNodes.GetLength(1))
            {
                //addingNode is difference to this node, so we add its changes.
                this.addedCountLayer.directionalNodes[indexX, indexY] += addingNode;

                //Get current layer diff
                this.directionLayer.directionalNodes[indexX, indexY] = this.addedCountLayer.directionalNodes[indexX, indexY] ^ this.defaultNode;
            }
        }

        public bool IsIndexPointInLayer(int indexX, int indexY)
        {
            if (indexX < 0 ||
                indexX >= this.directionLayer.directionalNodes.GetLength(0) ||
                indexY < 0 ||
                indexY >= this.directionLayer.directionalNodes.GetLength(1))
            {
                return false;
            }
            return true;
        }
    }
}