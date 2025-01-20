namespace CCompilerNs
{
    public class MainUt : BaseUt
    {
        public void gv_1()
        {
            string src = @"
int a;
char b;
int c = 3;
char d = 4;

int main()
{
	return d - c - a - b;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 1);
        }

        public void gv_2()
        {
            string src = @"

int a;
char b;
int c = 3;
char d = 4;
char e = 'e';

int main()
{
	return d - c + b - a + e;
}



";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 102);
        }

        public void gv_3()
        {
            string src = @"

int a[2][3];
int b[3];
char c[2][3];

int main()
{
    a[1][1] = 7;
    a[1][2] = 8;

    b[1] = 5;
    b[2] = 6;

    c[1][1] = 'A';
    c[1][2] = 'B';

    return a[1][1] + b[2] + c[1][2];
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 79);
        }

        public void gv_4()
        {
            string src = @"

int a[2][3];

void f1(int[][3] a)
{
    a[1][2] = 99;
}

void f2(int[][3] b)
{
    b[1][1] = 90;
}

int main()
{
    f1(a);
    f2(a);

    return a[1][2] - a[1][1];
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 9);
        }

        public void Ut1()
        {
            string src = @"
int main()
{
	return 77;
}
";
            AsmEmitter.SetOutputFile("test.s");

            Program program = (Program)cc.Parse(src);
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 77);
        }

        public void Ut2()
        {
            string src = @"
int main()
{
	return 1 + 5 - 2;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 4);
        }

        public void Ut3()
        {
            string src = @"
int main()
{
	return 1 - 7 + 11 - 20 - 44 + 89;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 30);
        }

        public void Ut4()
        {
            string src = @"
int main()
{
	return 2 * 3 - 8 / 4;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 4);
        }

        public void Ut5()
        {
            string src = @"
int main()
{
	return 2 - (3 - 11) / 2 + 5 * 6 - 7 * 2 + 9;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 31);
        }

        public void Ut6()
        {
            string src = @"
int main()
{
	return (10 + 20) * (30 − 5);
}
";

            // return 1 * (2-1);
            //return ((10 + 20) * (30−5))/ (15 + 5)−(25−10);
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 22);
        }

        public void Ut7()
        {
            string src = @"
int main()
{
    int a = 1 + 2;
	return a;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 3);
        }

        public void Ut8()
        {
            string src = @"
int main()
{
    int a = 1;
    int b = 2;
    int c = 3;
	return (a + b * c - 1) / c;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 2);
        }

        public void Ut9()
        {
            string src = @"
int main()
{
    int a = 2;
    int b = 3;
    int c = 6;
    int d = 2;
    int e;
    int f;
    e = 5;
	return a - (b - 11) / a + e * c - 7 * 2 + 9;
}
";
            // replace e with 5 then it will pass
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 31);
        }

        public void Ut10()
        {
            string src = @"
int main()
{
    int a = 2;
    int b = 3;
    int c = 6;
    int d = 2;
    int e;
    int f;
    e = a + d + 1;
    int g;
    g = 110;
    g = 11;
    int h = c + 1 + a - d;
	return a - (b - g) / a + e * c - h * a + (b + a * b);
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 31);
        }

        public void call_function_4()
        {
            string src = @"
int f1()
{
    return 1;
}

int f2()
{
    return 2;
}

int f3()
{
    return 3;
}

int main()
{
    return f1() + f2() * f3() + 10;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 17);
        }

        public void call_function_5()
        {
            string src = @"
int f1(int a, char b, int c, char d)
{
    return a + b + c + d;
}

int main()
{
    return f1(1, 'A', 2, 'B');
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 134);
        }

        public void call_function_2()
        {
            string src = @"
int f1(int a, int b, int c)
{
    return a + b + c;
}
int main()
{
    return f1(1, 2, 3);
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 6);
        }

        public void call_function_3()
        {
            string src = @"
int f1(int a, int b, int c)
{
    return a + b + c;
}

int f2()
{
    return 5;
}

int main()
{
    return f1(2 * 3,  1 + 2 * 4, f2());
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 20);
        }

        public void call_function_1()
        {
            string src = @"
void f1()
{
    return;
}

int main()
{
    f1();

    return 1;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 1);
        }

        public void Ut15()
        {
            string src = @"
int main()
{
    int a = 0;
    int b[1];

    a += 10;
    a -= 2;
    a *= 3;
    a /= 6;
    a++;
    a++;
    a++;
    a--;

    b[0] += 10;
    b[0] -= 2;
    b[0] *= 3;
    b[0] /= 6;
    b[0]++;
    b[0]++;
    b[0]++;
    b[0]--;

    return a + b[0];
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 12);
        }

        public void if_1()
        {
            string src = @"
int main()
{
    if (2 > 1)
{
        return 3;
}

    return 2;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 3);
        }

        public void if_2()
        {
            string src = @"
int fib(int n) {
    if (n <= 1)
{
        return n;
}

    return fib(n - 1) + fib(n - 2);
}

int main() {
    return fib(10);
}
";

            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 55);
        }

        public void if_3()
        {
            string src = @"
int main() {
    if (1 > 2)
        return 1;
    else if (2 > 2)
        return 2;
    else if (3 > 2)
        return 3;


    return 0;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 3);
        }

        public void if_4()
        {
            string src = @"
int main() {
    if (1 > 2)
        return 1;
    else
        return 2;


    return 0;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 2);
        }

        public void if_5()
        {
            string src = @"
int main() {
    if (1 > 5)
        return 1;
    else if (2 > 5)
        return 2;
else
    return 3;


    return 0;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 3);
        }

        public void if_6()
        {
            string src = @"
int main() {
    if (1 > 5)
        return 1;
    else if (2 > 5)
        return 2;
    else if (3 > 5)
        return 3;
    else
        return 4;

    return 0;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 4);
        }

        public void for_1()
        {
            string src = @"
int main() {

    int a = 0;
    int i = 0;
    for (i = 1; i <= 10; i++)
{
    a = a + i;
}

    return a;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 55);
        }

        public void for_2()
        {
            string src = @"
int main() {

    int a = 0;
    int i = 0;
    for (i = 1; i <= 10; i++)
{
    if (i == 5)
    {
       continue;
    }

    a += i;
}

    return a;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 50);
        }

        public void for_3()
        {
            string src = @"
int main() {

    int a = 0;
    int i = 0;
    for (i = 1; i <= 10; i++)
{
    if (i == 5)
       break;

    a += i;
}

    return a;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 10);
        }

        public void for_4()
        {
            string src = @"
int main() {

    int a = 0;
    int i = 0;
    for (i = 1; i <= 10; i++)
{
    if (i == 5)
    {
       break;
    }

    if (i == 3)
    {
       continue;
    }

    a += i;
}

    return a;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 7);
        }

        public void for_5()
        {
            string src = @"
int main() {

    int i = 0;
    int j = 0;
    int k = 0;

    for (i = 0; i < 10; i++)
        for (j = 0; j < 10; j++)
        {
             if (j == 9)
                 break;

             k += 1;
        }

    return k;
}
";

            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 90);
        }

        public void for_6()
        {
            string src = @"
int main() {
    int i = 0;
    int j = 0;
    int k = 0;
    int a[3][4][5];

    for (i = 0; i < 3; i++)
        for (j = 0; j < 4; j++)
           for (k = 0; k < 5; k++)
               a[i][j][k] = i * 4 * 5 + j * 5 + k;

    return a[2][3][4];
}
";

            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 59);
        }


        public void array_1()
        {
            string src = @"
int main() {
    int a[4];

    a[2] = 3;
    

    return a[2];
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 3);
        }

        public void array_2()
        {
            string src = @"
int main() {
    int a[5][4];

    a[3][2] = 9;   

    return a[3][2] - 2;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 7);
        }

        public void array_3()
        {
            string src = @"
int main() {
    int a[5][4][3];

    a[4][3][2] = 11;   

    return a[4][3][2] - 2;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 9);
        }

        public void array_4()
        {
            string src = @"
int main() {
    int a[5][4];
    int i = 4;
    int j = 3;
    a[i][j] = 8;

    return a[2 * 2][ 4 - 2 + 1];
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 8);
        }

        public void array_5()
        {
            string src = @"
int main() {
    int a[5][4][3];
    int i;
    int j;
    int k;
    int sum = 0;

    for (i = 0; i < 5; i = i + 1)
        for (j = 0; j < 4; j = j + 1)
            for (k = 0; k < 3; k = k + 1)
                a[i][j][k] = 2;

    a[3][2][1] = a[3][2][1] + 1;
    a[2][2][2] = a[2][2][2] + 2;
    a[1][1][1] = a[1][1][1] * 3;

    for (i = 0; i < 5; i = i + 1)
        for (j = 0; j < 4; j = j + 1)
            for (k = 0; k < 3; k = k + 1)
                sum = sum + a[i][j][k];

    return sum;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 127);
        }

        public void array_6()
        {
            string src = @"
void f1(int[] a, int len) {
    int i = 0;
    
    for (i = 0; i < len; i++)
        a[i]++;
}

int main() {
    int a[5];
    int i;

    for (i = 0; i < 5; i++)
        a[i] = i * 2;

    f1(a, 5);
    return a[3];
}
";
            /*

             */

            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 7);
        }

        public void array_7()
        {
            string src = @"
void f1(int[][4][5] a, char[][4][5] b) 
{
    int i = 0;
    int j = 0;
    int k = 0;
    
    for (i = 0; i < 3; i++)
        for (j = 0; j < 4; j++)
            for (k = 0; k < 5; k++)
            {
                a[i][j][k] += (i * 4 * 5 + j * 5 + k) * 3;
                b[i][j][k] += (i * 4 * 5 + j * 5 + k) * 2; 
            }
}

int main() {
    int i = 0;
    int j = 0;
    int k = 0;
    int a[3][4][5];
    char b[3][4][5];

    for (i = 0; i < 3; i++)
        for (j = 0; j < 4; j++)
            for (k = 0; k < 5; k++)
            {
                a[i][j][k] = (i * 4 * 5 + j * 5 + k) * 2;
                b[i][j][k] = i * 4 * 5 + j * 5 + k; 
            }

    f1(a, b);
    return a[1][2][3] - b[0][1][2];
}
";
            // 165 - 21

            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 144);
        }

        public void array_8()
        {
            string src = @"
int f1(int[][4][5] a, char[][4][5] b) 
{
    int i = 0;
    int j = 0;
    int k = 0;
    
    for (i = 0; i < 3; i++)
        for (j = 0; j < 4; j++)
            for (k = 0; k < 5; k++)
            {
                a[i][j][k] += (i * 4 * 5 + j * 5 + k) * 3;
                b[i][j][k] += (i * 4 * 5 + j * 5 + k) * 2; 
            }
    return b[0][1][3];
}

int main() {
    int i = 0;
    int j = 0;
    int k = 0;
    int l;
    int a[3][4][5];
    char b[3][4][5];

    for (i = 0; i < 3; i++)
        for (j = 0; j < 4; j++)
            for (k = 0; k < 5; k++)
            {
                a[i][j][k] = (i * 4 * 5 + j * 5 + k) * 2;
                b[i][j][k] = i * 4 * 5 + j * 5 + k; 
            }

    l = f1(a, b);
    return l + a[1][2][3] - b[0][1][2];
}
";
            // 24 + 165 - 21

            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 168);
        }

        public void char_1()
        {
            string src = @"
int main() {
    char a;
    char b = 1;
    char c = 'c';
    char d = 'd';

    return d - c + b;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 2);
        }

        public void char_2()
        {
            string src = @"
int main() {
    char a[10];
    a[3] = 'a';
    a[4] = 'b';
    a[5] = 'c';

    return a[5] -a[3];
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 2);
        }

        public void char_3()
        {
            string src = @"
int main() {
    int b =6;
    char a[10];
    a[3] = 'a';
    a[4] = 'b';
    a[5] = 'c';

    return b + a[5] -a[3];
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 8);
        }

        public void char_4()
        {
            string src = @"
int main() {
    int i;
    int j;
    int k;
    char a[2][3][4];

    for (i = 0; i < 2; i++)
        for (j = 0;j < 3;j++)
            for (k = 0; k < 4; k++)
                a[i][j][k] = 'a' + i * 3 * 4 + j * 4 + k;


    return a[1][2][3];
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 120);
        }

        public void empty_1()
        {
            string src = @"
void f1()
{
}

int main() {
    int i;

    if (1 > 0)
    {
    } 
    else if (1 > 0)
    {
    }
    else if (1 > 0)
    {
    }
    else
    {
    }

    for (i = 0; i < 1; i++)
    {
    }

    return 0;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 0);
        }

        public void empty_2()
        {
            string src = @"
void f1()
{
    ;
}

void f2()
{
    ;;;;;
}

void f3()
{
    int a;;;;;;;
}

int main() {
    int i;

    if (1 > 0)
    ;
    else if (1 > 0)
    ;
    else if (1 > 0)
    ;
    else
    ;

    for (i = 0; i < 1; i++)
    ;

    return 0;
}
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 0);
        }

        public void comment_1()
        {
            string src = @"

// qq
int a = 1; // qqqwer
// qqq int b = 2;
// xdxxxd
// aaa

int main() {
//aaa
    return 0; // qqqwer
////
///xxxx
}
// asdfasf;
";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 0);
        }

        public void struct_1()
        {
            string src = @"

struct A {
    int a1;
    char a2;
};

struct B {
    int b1;
    char b2;
    struct A b3;  
};

int main()
{
    char e;
    struct A x;
    int c;
    int d;

    return 0;
}

";
            AsmEmitter.SetOutputFile("test.s");

            object ret = cc.Parse(src);
            Program program = (Program)ret;
            program.Print();

            program.EmitAsm();

            int exitCode = CompileAndRun("test.s", "test.exe");
            Check(exitCode == 0);
        }

        public void adhoc()
        {

        }

        public static void RunAllUt()
        {
            MainUt mainUt = new MainUt();

            mainUt.adhoc();

            mainUt.struct_1();

            mainUt.Ut1();
            mainUt.Ut2();
            mainUt.Ut3();
            mainUt.Ut4();
            mainUt.Ut5();
            //mainUt.Ut6();
            mainUt.Ut7();
            mainUt.Ut8();
            mainUt.Ut9();
            mainUt.Ut10();

            mainUt.call_function_1();
            mainUt.call_function_2();
            mainUt.call_function_3();
            mainUt.call_function_4();
            mainUt.call_function_5();

            mainUt.Ut15();

            mainUt.if_1();
            mainUt.if_2();
            mainUt.if_3();
            mainUt.if_4();
            mainUt.if_5();
            mainUt.if_6();

            mainUt.for_1();
            mainUt.for_2();
            mainUt.for_3();
            mainUt.for_4();
            mainUt.for_5();
            mainUt.for_6();

            mainUt.array_1();
            mainUt.array_2();
            mainUt.array_3();
            mainUt.array_4();
            mainUt.array_5();
            mainUt.array_6();
            mainUt.array_7();
            mainUt.array_8();

            mainUt.char_1();
            mainUt.char_2();
            mainUt.char_3();
            mainUt.char_4();

            mainUt.gv_1();
            mainUt.gv_2();
            mainUt.gv_3();
            mainUt.gv_4();

            mainUt.empty_1();
            mainUt.empty_2();

            mainUt.comment_1();
        }

        public static void Ut()
        {
            RunAllUt();
        }
    }
}
