using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;



namespace ConsoleApp49
{
    class Vector2
    {
        public float x = 0;
        public float y = 0;
        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }


    class Bone
    {
        public float angle { get; set; } = 0;
        public float length { get; set; } = 0;
        public int ID = 0;
        public Bone nextBone { get; set; } = null;
        public Bone(float angle, float length, int ID)
        {
            this.angle = angle;
            this.length = length;
            this.ID = ID;
        }
        public Bone()
        {

        }
    }

    class BoneManager
    {
        Bone baseBone = new Bone();
        public BoneManager(Bone baseBone)
        {
            this.baseBone = baseBone;
        }
        public Vector2 GetPositon(int id)
        {
            Vector2 pos = new Vector2(0, 0);
            Bone b = baseBone;
            float angle = 0;
            while (b.ID != id)
            {
                angle += b.angle;
                pos.x += MathF.Cos(angle) * b.length;
                pos.y += MathF.Sin(angle) * b.length;
                b = b.nextBone;
            }
            angle += b.angle;
            pos.x += MathF.Cos(angle) * b.length;
            pos.y += MathF.Sin(angle) * b.length;
            return pos;
        }

        public void SetPosition(Vector2 position, int ID)
        {
            Vector2 lastPosition = GetPositon(ID);
            Vector2 dif = new Vector2(position.x - lastPosition.x, position.y - lastPosition.y);
            float diffVal = dif.x * dif.x + dif.y * dif.y;
            float h = 0.00001f;
            int j = 0;
            while (true)
            {

                float diffVal2 = 0;
                float grad = 0;
                Bone b = baseBone;
                List<float> list = new List<float>();
                while (b.ID != ID)
                {
                    b.angle += h;
                    lastPosition = GetPositon(ID);
                    b.angle -= h;
                    dif = new Vector2(position.x - lastPosition.x, position.y - lastPosition.y);
                    diffVal2 = dif.x * dif.x + dif.y * dif.y;
                    grad = (-diffVal + diffVal2) / h;
                    list.Add(grad * 0.00001f);
                    b = b.nextBone;

                }
                b.angle += h;
                lastPosition = GetPositon(ID);
                b.angle -= h;
                dif = new Vector2(position.x - lastPosition.x, position.y - lastPosition.y);
                diffVal2 = dif.x * dif.x + dif.y * dif.y;
                grad = (-diffVal + diffVal2) / h;
                list.Add(grad * 0.00001f);

                b = baseBone;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] < -0.1f || list[i] > 0.1f)
                    {
                        b.angle -= MathF.Sign(list[i]) * 0.1f;
                    }
                    else
                        b.angle -= list[i];
                    Console.Write(b.angle + " ");

                    b = b.nextBone;
                }
                Console.WriteLine();
                lastPosition = GetPositon(ID);
                dif = new Vector2(position.x - lastPosition.x, position.y - lastPosition.y);
                diffVal = dif.x * dif.x + dif.y * dif.y;

                j++;
                if (diffVal < 0.01f)
                {
                    Console.WriteLine(lastPosition.x + "," + lastPosition.y);
                    Console.WriteLine(j);
                    break;
                }
            }

        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Bone b0 = new Bone(MathF.PI / 2.0f, 100, 0);
            Bone b1 = new Bone(-MathF.PI / 2.0f, 50, 1);
            Bone b2 = new Bone(0, 25, 2);
            Bone b3 = new Bone(0, 10, 3);
            b0.nextBone = b1;
            b1.nextBone = b2;
            b2.nextBone = b3;
            b3.nextBone = null;

            var bManager = new BoneManager(b0);
            var pos = bManager.GetPositon(3);

            while (true)
            {
                int x = int.Parse(Console.ReadLine());
                int y = int.Parse(Console.ReadLine());
                bManager.SetPosition(new Vector2(x, y), 3);
            }
        }
    }
}
