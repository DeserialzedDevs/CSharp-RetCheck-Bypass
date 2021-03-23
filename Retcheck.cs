using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EyeStepPackage;

namespace RetCheckBypass
{
    class RetCheck
    {
        private static int FindNextJB(int addr)
        {
            int func_sz = util.nextPrologue(addr) - addr;
            int iteration = 0;
            while (func_sz > iteration)
            {
                var e = EyeStep.read(addr + iteration);
                if (e.info.opcode_name == "jb short")
                {
                    break;
                }
                else if (util.isEpilogue(addr + iteration))
                {
                    return 0;
                }
                iteration += e.len;
            }
            return iteration + addr;
        }

        private static int[] FindAllJB(int addr)
        {
            int[] JBs = { };
            int func_sz = util.nextPrologue(addr) - addr;
            int iteration = 0;
            while (true)
            {
                int JB = FindNextJB(iteration + addr);
                if (func_sz < JB || JB == 0)
                {
                    break;
                }
                JBs[iteration++] = addr + JB;
            }

            return JBs;
        }

        private static int FindNextJA(int addr)
        {
            int func_sz = util.nextPrologue(addr) - addr;
            int iteration = 0;
            while (func_sz > iteration)
            {
                var e = EyeStep.read(addr + iteration);
                if (e.info.opcode_name == "ja short")
                {
                    break;
                }
                else if (util.isEpilogue(addr + iteration))
                {
                    return 0;
                }
                iteration += e.len;
            }
            return iteration + addr;
        }

        private static int[] FindAllJA(int addr)
        {
            int[] JAs = { };
            int func_sz = util.nextPrologue(addr) - addr;
            int iteration = 0;
            while (true)
            {
                int JA = FindNextJA(iteration + addr);
                if (func_sz < JA || JA == 0)
                {
                    break;
                }
                JAs[iteration++] = addr + JA;
            }

            return JAs;
        }

        public static void PatchJBs(int func_addr)
        {
            var JBs = FindAllJB(func_addr);
            foreach (int JBAddr in JBs)
            {
                util.writeByte(JBAddr, 0x77);
            }
        }

        public static void PatchJAs(int func_addr)
        {
            var JAs = FindAllJA(func_addr);
            foreach (int JAAddr in JAs)
            {
                util.writeByte(JAAddr, 0x72);
            }
        }
    }
}
