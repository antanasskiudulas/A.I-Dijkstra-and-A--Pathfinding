using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace Pathfinder
{
    class Dijkstra
    {
        public bool[,] closed; //whether or not location is closed
        public float[,] cost; //cost value for each location
        public Coord2[,] link; //link for each location = coords of a neighbouring location
        public bool[,] inPath; //whether or not a locatio

        public Dijkstra()
        {
            closed = new bool[40, 40];
            cost = new float[40, 40];
            link = new Coord2[40, 40];
            inPath = new bool[40, 40];
        }

        public void Build(Level level, AiBotBase bot, Player plr)
        {
            for(int i = 0; i < 40; i++)
            {
                for (int x = 0; x < 40; x++)
                {
                    closed[i, x] = false;
                    cost[i, x] = 1000000;
                    link[i, x] = new Coord2(-1,-1);
                    inPath[i, x] = false;
                }
            }
            cost[bot.GridPosition.X, bot.GridPosition.Y] = 0;

            while(!closed[plr.GridPosition.X, plr.GridPosition.Y])
            {
                //finding the lowest cost square
                float min = 1000000;
                Coord2 pos = new Coord2(0, 0);
                for(int x = 0; x < 40; x++)
                {
                    for (int y = 0; y < 40; y++)
                    {
                        if(cost[x, y] < min && !closed[x, y])
                        {
                            min = cost[x, y];
                            pos.X = x;
                            pos.Y = y;
                        }
                    }
                }

                closed[pos.X, pos.Y] = true;
                //right
                if (level.ValidPosition(new Coord2(pos.X, pos.Y + 1)) && (cost[pos.X, pos.Y + 1] > cost[pos.X, pos.Y] + 1))
                {
                    cost[pos.X, pos.Y + 1] = cost[pos.X, pos.Y] + 1;
                    link[pos.X, pos.Y + 1] = pos;
                }
                //left
                if (level.ValidPosition(new Coord2(pos.X, pos.Y - 1)) && (cost[pos.X, pos.Y - 1] > cost[pos.X, pos.Y] + 1))
                {
                    cost[pos.X, pos.Y - 1] = cost[pos.X, pos.Y] + 1;
                    link[pos.X, pos.Y - 1] = pos;
                }
                //top
                if (level.ValidPosition(new Coord2(pos.X - 1, pos.Y)) && (cost[pos.X - 1, pos.Y] > cost[pos.X, pos.Y] + 1))
                {
                    cost[pos.X - 1, pos.Y] = cost[pos.X, pos.Y] + 1;
                    link[pos.X - 1, pos.Y] = pos;
                }
                //bot
                if (level.ValidPosition(new Coord2(pos.X + 1, pos.Y)) && (cost[pos.X + 1, pos.Y] > cost[pos.X, pos.Y] + 1))
                {
                    cost[pos.X + 1, pos.Y] = cost[pos.X, pos.Y] + 1;
                    link[pos.X + 1, pos.Y] = pos;
                }
                //top-left
                if (level.ValidPosition(new Coord2(pos.X - 1, pos.Y - 1)) && (cost[pos.X - 1, pos.Y - 1] > cost[pos.X, pos.Y] + 1.4f))
                {
                    cost[pos.X - 1, pos.Y - 1] = cost[pos.X, pos.Y] + 1.4f;
                    link[pos.X - 1, pos.Y - 1] = pos;
                }
                //top-right
                if (level.ValidPosition(new Coord2(pos.X - 1, pos.Y + 1)) && (cost[pos.X - 1, pos.Y + 1] > cost[pos.X, pos.Y] + 1.4f))
                {
                    cost[pos.X - 1, pos.Y + 1] = cost[pos.X, pos.Y] + 1.4f;
                    link[pos.X - 1, pos.Y + 1] = pos;
                }
                //bot-left
                if (level.ValidPosition(new Coord2(pos.X + 1, pos.Y - 1)) && (cost[pos.X + 1, pos.Y - 1] > cost[pos.X, pos.Y] + 1.4f))
                {
                    cost[pos.X + 1, pos.Y - 1] = cost[pos.X, pos.Y] + 1.4f;
                    link[pos.X + 1, pos.Y - 1] = pos;
                }
                //bot-right
                if (level.ValidPosition(new Coord2(pos.X + 1, pos.Y + 1)) && (cost[pos.X + 1, pos.Y + 1] > cost[pos.X, pos.Y] + 1.4f))
                {
                    cost[pos.X + 1, pos.Y + 1] = cost[pos.X, pos.Y] + 1.4f;
                    link[pos.X + 1, pos.Y + 1] = pos;
                }
            }

            bool done = false; //set to true when we are back at the bot position
            Coord2 nextClosed = plr.GridPosition; //start of path
            while (!done)
            {
                inPath[nextClosed.X, nextClosed.Y] = true;
                nextClosed = link[nextClosed.X, nextClosed.Y];
                if (nextClosed == bot.GridPosition)
                    done = true;
            }
        }
    }
}
