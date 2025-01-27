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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 31);
        }

        public void Ut6()
        {
            string src = @"
int main()
{
	return (10 + 20) * (30 - 5);
}
";
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 750);
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 31);
        }

        public void call_function_6()
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 17);
        }

        public void call_function_7()
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 134);
        }

        public void call_function_4()
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 6);
        }

        public void call_function_5()
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 1);
        }

        public void call_function_2()
        {
            string src = @"
void f1(int a)
{
    return a;
}

int main()
{
    return f1(7);
}
";
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 7);
        }

        public void call_function_3()
        {
            string src = @"
void f1(int a, int b)
{
    return a + b;
}

int main()
{
    return f1(7, 9);
}
";
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 16);
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

    b[0] = 0;
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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

            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 4);
        }

        public void if_7()
        {
            string src = @"
int main() {
    int i = 3;
    int j = 0;

    while (i)
    {
        j += i;
        i--;
    }

    return j;
}
";
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 6);
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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

            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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

            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 59);
        }

        public void for_7()
        {
            string src = @"
int main() {

    int a = 0;
    int i = 1;
    while(i <= 10)
{
    a = a + i;
    i++;
}

    return a;
}
";
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 55);
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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

            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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

            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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

            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 0);
        }

        public void comment_2()
        {
            string src = @"
/*
int a = 1;
qwer
qqq
*/

int main() {
/* asdf */

// /*
/* //
*/
    return 0;
}
/*
qqq
*/

";
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 0);
        }

        public void call_c_function_1()
        {
            string src = @"

char a[5];
int x = 10;

int main()
{
    int y = 20;
    a[0] ='h';
    a[1] ='i';
    a[2] ='!';
    a[3] = 0;

    strlen(a);
    strlen(a);
    strlen(a);

    return x + y + strlen(a);
}

";
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 33);
        }

        public void call_c_function_2()
        {
            string src = @"

char a[10];

int main()
{
    a[0] ='h';
    a[1] ='i';
    a[2] ='!';

    printf(""%s"", a);

    return 0;
}

";
            Compiler.GenerateAsm(src, "test.s");
            Tuple<int, string> ret2 = CompileAndRun2("test.s", "test.exe");
            int exitCode = ret2.Item1;
            string output = ret2.Item2;

            Check(exitCode == 0);
            Check(output.Trim() == "hi!");
        }

        public void call_c_function_3()
        {
            string src = @"
int main()
{
    printf(""%d,%d,%d,%d,%d,%d,%d,"", 1, 2, 3, 4, 5, 6, 7);

    return 0;
}

";
            Compiler.GenerateAsm(src, "test.s");
            Tuple<int, string> ret2 = CompileAndRun2("test.s", "test.exe");
            int exitCode = ret2.Item1;
            string output = ret2.Item2;

            Check(exitCode == 0);
            Check(output.Trim() == "1,2,3,4,5,6,7,");
        }

        public void call_c_function_4()
        {
            string src = @"

char format[100];

int main()
{
    char a[10];
    char b[10];

    strcpy(a, ""hi"");
    strcpy(b, ""hello"");

    printf(""%s,"", a);
    printf(""%s,"", b);
    strcpy(a, b);
    printf(""%s"", a);

    return 99;
}

";
            Compiler.GenerateAsm(src, "test.s");
            Tuple<int, string> ret2 = CompileAndRun2("test.s", "test.exe");
            int exitCode = ret2.Item1;
            string output = ret2.Item2;

            Check(exitCode == 99);
            Check(output.Trim() == "hi,hello,hello");
        }

        public void call_c_function_5()
        {
            string src = @"

char a[100];
char b[100];

int main()
{
    strcpy(a, ""hi"");
    strcpy(b, "", how are you?"");
    strcat(a, b);
    printf(a);

    return 0;
}

";
            Compiler.GenerateAsm(src, "test.s");
            Tuple<int, string> ret2 = CompileAndRun2("test.s", "test.exe");
            int exitCode = ret2.Item1;
            string output = ret2.Item2;

            Check(exitCode == 0);
            Check(output.Trim() == "hi, how are you?");
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
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 0);
        }

        public void struct_2()
        {
            string src = @"

struct Z {
    char z1;
    int z2;
};

struct A {
    int a1;
    char a2;
    struct Z a3;
};

struct B {
    int b1;
    char b2;
    struct A b3;  
    int b4;
    char b5;
};

int main()
{
    char e;
    struct B x;
    int c;
    int d;

    x.b1 = 1;
    x.b2 = 3;
    x.b4 = 40;
    x.b5 = 70;
    x.b3.a1 = 7;
    x.b3.a2 = 20;
    x.b3.a3.z1 = 6;
    x.b3.a3.z2 = 5;

    return x.b1 + x.b2 + x.b3.a1 + x.b3.a2 + x.b4 + x.b5 + x.b3.a3.z1 + x.b3.a3.z2;
}

";
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 152);
        }

        public void struct_3()
        {
            string src = @"

struct A {
    int a1;
    char a2;
};

int f1(int a, char b)
{
    return a + b;
}

int main()
{
    struct A a;
    a.a1 = 1;
    a.a2 = 2;

    return f1(a.a1, a.a2);
}

";
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 3);
        }

        public void struct_4()
        {
            string src = @"

struct A {
    int a1;
    char a2;
};

struct A a;

int f1(int b, char c)
{
    return b + c;
}

int main()
{
    a.a1 = 3;
    a.a2 = 4;

    return f1(a.a1, a.a2);
}

";
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 7);
        }

        public void struct_5()
        {
            string src = @"

struct A {
    int a1;
    char a2;
    int a3;
};

int f1(struct A a)
{
    return a.a3 - a.a2 - a.a1;
}

int main()
{
    struct A a;
    a.a1 = 1;
    a.a2 = 5;
    a.a3 = 10;

    return f1(a);
}

";
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 4);
        }

        public void struct_6()
        {
            string src = @"

struct A {
    int a1;
    char a2;
    int a3;
};

struct B {
    int b1;
    char b2;
};

int f1(struct A a, struct B b)
{
    return a.a3 - a.a2 - a.a1 - b.b1 - b.b2;
}

int main()
{
    struct A a;
    a.a1 = 8;
    a.a2 = 10;
    a.a3 = 30;

    struct B b;
    b.b1 = 2;
    b.b2 = 5;

    return f1(a, b);
}

";
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 5);
        }

        public void struct_7()
        {
            string src = @"

struct B {
    int b1;
    int b2;
};

struct A {
    int a1;
    struct B a2[3];
    char a3;
};

int main()
{
    struct A a;
    a.a2[1].b1 = 2;
    a.a2[1].b2 = 5;
    a.a2[2].b1 = 10;
    a.a2[2].b2 = 20;

    return a.a2[1].b1 + a.a2[1].b2 + a.a2[2].b1 + a.a2[2].b2;
}

";
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 37);
        }

        public void struct_8()
        {
            string src = @"

struct C {
    char c1;
    char c2;
    char c3;
};

struct B {
    int b1;
    int b2;
    struct C b3[2][3];
};

struct A {
    int a1;
    struct B a2[5];
    char a3;
};

int main()
{
    struct A a[3];
    a[2].a2[3].b3[1][2].c1 = 5;
    a[2].a2[3].b3[1][2].c2 = 7;
    a[2].a2[3].b3[1][2].c3 = 9;

    a[1].a2[2].b3[2][2].c3 = 15;

    return a[2].a2[3].b3[1][2].c1 + a[2].a2[3].b3[1][2].c3 + a[1].a2[2].b3[2][2].c3;
}

";
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 29);
        }

        public void struct_9()
        {
            string src = @"

struct C {
    char c1;
    char c2;
    char c3;
};

struct B {
    int b1;
    int b2;
    struct C b3[2][3];
};

struct A {
    int a1;
    struct B a2[5];
    char a3;
};

struct A a[3];

int main()
{
    a[2].a2[3].b3[1][2].c1 = 5;
    a[2].a2[3].b3[1][2].c2 = 7;
    a[2].a2[3].b3[1][2].c3 = 9;

    a[1].a2[2].b3[2][2].c3 = 15;

    return a[2].a2[3].b3[1][2].c1 + a[2].a2[3].b3[1][2].c3 + a[1].a2[2].b3[2][2].c3;
}

";
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 29);
        }

        public void struct_10()
        {
            string src = @"

struct C {
    char c1;
    char c2;
    char c3;
};

struct B {
    int b1;
    int b2;
    struct C b3[2][3];
};

struct A {
    int a1;
    struct B a2[5];
    char a3;
};

int f1(struct A a[])
{
    a[2].a2[3].b3[1][2].c1 = 5;
    a[2].a2[3].b3[1][2].c2 = 7;
    a[2].a2[3].b3[1][2].c3 = 9;

    a[1].a2[2].b3[2][2].c3 = 15;

    return a[2].a2[3].b3[1][2].c1 + a[2].a2[3].b3[1][2].c3 + a[1].a2[2].b3[2][2].c3;
}

int main()
{
    struct A a[3];

    return f1(a);
}

";
            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 29);
        }


        public void boolean_expression_1()
        {
            string src = @"

int main()
{
    if (1)
        return 1;
    else
        return 2;
}

";

            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 1);
        }

        public void boolean_expression_2()
        {
            string src = @"

int main()
{
    if (0)
        return 1;
    else
        return 2;
}

";

            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 2);
        }

        public void boolean_expression_3()
        {
            string src = @"

int main()
{
    if (2 > 3 || 3 > 2 )
        return 1;
    else
        return 2;
}

";

            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 1);
        }

        public void boolean_expression_4()
        {
            string src = @"

int main()
{
    if (1 > 2 || 3 > 4 )
        return 1;
    else
        return 2;
}

";

            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 2);
        }

        public void boolean_expression_5()
        {
            string src = @"

int main()
{
    if (2 < 1 && 3 < 1 && 1 > 2 || 5 > 6 && 3 > 4 || 2 > 1 && 3 > 1 )
        return 1;
    else
        return 2;
}

";

            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 1);
        }

        public void boolean_expression_6()
        {
            string src = @"

int main()
{
    if (2 < 1 && 3 < 1 && 1 > 2 || 5 > 6 && 3 > 4 || 2 > 1 && 3 > 1 && 0 )
        return 1;
    else
        return 2;
}

";

            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 2);
        }

        public void boolean_expression_7()
        {
            string src = @"

int main()
{
    if (1 || 0)
        return 1;
    else
        return 2;
}

";

            Compiler.GenerateAsm(src, "test.s");
            int exitCode = CompileAsmAndRun("test.s", "test.exe");
            Check(exitCode == 1);
        }

        public void adhoc()
        {
            string src = @"


int main() {

    return 0;
}
";
            Compiler.GenerateAsm(src, "test.s");
            Tuple<int, string> ret2 = CompileAndRun2("test.s", "test.exe");
            int exitCode = ret2.Item1;
            string output = ret2.Item2;

            //Check(exitCode == 3);
        }

        public void pointer_1()
        {
            string src = @"
int* a = 0;
int** b = 0;
int c = 2;

int main() {
    a = &c;
    b = &a;
    int* d = &a;
    
    a = *b;
    c = **b;

    a = &c;
    *a = 5;
    **b = 6;

    return 0;
}
";
            Compiler.GenerateAsm(src, "test.s");
            Tuple<int, string> ret2 = CompileAndRun2("test.s", "test.exe");
            int exitCode = ret2.Item1;
            string output = ret2.Item2;

            Check(exitCode == 0);
        }

        public void pointer_2()
        {
            string src = @"
int main() {
int* a = 0;
int** b = 0;
int c = 77;
    a = &c;
    b = &a;
    int* d = &a;

    if (b != 0)
        ;
    else
        return 1;

    if (b == d)
        ;
    else
        return 2;
    
    a = *b;
    if (*a == 77)
        ;
    else
        return 3;

    c = **b;
    if (c == 77)
        ;
    else
        return 4;

    a = &c;
    b = &a;

    *a = 8;
    if (c == 8)
        ;
    else
        return 5;

    **b = 3;
    if (c == 3)
        ;
    else
        return 6;

    return 0;
}
";
            Compiler.GenerateAsm(src, "test.s");
            Tuple<int, string> ret2 = CompileAndRun2("test.s", "test.exe");
            int exitCode = ret2.Item1;
            string output = ret2.Item2;

            Check(exitCode == 0);
        }

        public void pointer_3()
        {
            string src = @"

int f1(int* a, int** b)
{
    int c = 100;
    a = *b;
    if (*a == 77)
        ;
    else
        return 3;

    c = **b;
    if (c == 77)
        ;
    else
        return 4;

    **b = 3;

    return 0;
}

int main() {
    int* a = 0;
    int** b = 0;
    int c = 77;
    a = &c;
    b = &a;
    
    int ret = f1(a, b);

    if (ret != 0)
        return ret;

    if (c == 3)
        ;
    else
        return 7;

    return 0;
}
";
            Compiler.GenerateAsm(src, "test.s");
            Tuple<int, string> ret2 = CompileAndRun2("test.s", "test.exe");
            int exitCode = ret2.Item1;
            string output = ret2.Item2;

            Check(exitCode == 0);
        }

        public void pointer_4()
        {
            string src = @"

char c[20];

int main() {
    char* a = malloc(10);
    strcpy(a, ""hi"");
    printf(""%s\n"", a);

    char* b = ""hello"";
    printf(""%s\n"", b);

    sprintf(c, ""%s!%s!"", a, b);

    printf(""%s\n"", c);

    return 0;
}
";
            Compiler.GenerateAsm(src, "test.s");
            Tuple<int, string> ret2 = CompileAndRun2("test.s", "test.exe");
            int exitCode = ret2.Item1;
            string output = ret2.Item2;

            Check(exitCode == 0);
            Check(output.Contains("hi"));
            Check(output.Contains("hello"));
            Check(output.Contains("hi!hello!"));
        }

        public void pointer_5()
        {
            string src = @"

struct A {
    int a0;
    int a1;
    int a2;
};

struct B {
    int b0;
    struct A *b1;
    int b2;
    int b3;
};

int main() {
    
    struct B b;
    struct A a;
    b.b1 = a;
    a.a1 = 9;

    if (b.b1->a1 == 9)
        ;
    else
        return 1;

    b.b1->a1 = 7;

    if (a.a1 == 7)
        ;
    else
        return 2;

    return 0;
}
";
            Compiler.GenerateAsm(src, "test.s");
            Tuple<int, string> ret2 = CompileAndRun2("test.s", "test.exe");
            int exitCode = ret2.Item1;
            string output = ret2.Item2;

            Check(exitCode == 0);
        }

        public void pointer_6()
        {
            string src = @"

struct A {
    int* a0;
    int a1;
    int a2;
};

struct B {
    int b0;
    struct A *b1;
    int* b2;
    int b3;
};

struct C {
    int c1;
    struct B *c2;
};

int main() {
    
    struct C c;
    struct B b;
    struct A a;
    int x = 8;
    int *y = 0;

    c.c2 = &b;
    b.b1 = &a;

    c.c2->b3 = 9;
    if (c.c2->b3 == 9 && b.b3 == 9)
        ;
    else
        return 1;

    c.c2->b1->a1 = 2;


    if (c.c2->b1->a1 == 2 && a.a1 == 2)
        ;
    else
        return 2;

    c.c2->b1->a1 = &x;
    *c.c2->b1->a1 = 4;

    if (*c.c2->b1->a1 == 4 && x == 4)
        ;
    else
        return 3;

    y = &c.c2->b1->a2;
    *y = 5;

    if (c.c2->b1->a2 == 5 && *y == 5)
        ;
    else
        return 4;

    printf(""%d"", *c.c2->b1->a1);
    
    return 0;
}
";
            Compiler.GenerateAsm(src, "test.s");
            Tuple<int, string> ret2 = CompileAndRun2("test.s", "test.exe");
            int exitCode = ret2.Item1;
            string output = ret2.Item2;

            Check(exitCode == 0);
        }

        public void EightQueen()
        {
            string src = @"

int N = 8;
int board[8];

int is_safe(int row, int col) {
    int i = 0;

    for (i = 0; i < row; i++) {
        if (board[i] == col)
            return 0;
        if (board[i] - i == col - row)
            return 0;
        if (board[i] + i == col + row)
            return 0;
    }
    return 1;
}

int solve(int row) {
    int i = 0;
    int j = 0;

    if (row == N) {
        for (i = 0; i < N; i++) {
            for (j = 0; j < N; j++) {
                if (board[i] == j) {
                    printf(""Q "");
                }
                else {
                    printf("". "");
                }
            }
            printf(""\n"");
        }
        printf(""\n"");
        return 1;
    }
    
    int count = 0;
    int col = 0;
    for (col = 0; col < N; col++) {
        if (is_safe(row, col)) {
            board[row] = col;
            count += solve(row + 1);
        }
    }

    return count;
}

int main() {
    int i = 0;
    int count = 0;

    for (i = 0; i < N; i++)
        board[i] = -1;

    count = solve(0);
    printf(""Solution count = %d\n"", count);

    return 0;
}

";
            Compiler.GenerateAsm(src, "test.s");
            Tuple<int, string> ret2 = CompileAndRun2("test.s", "test.exe");
            int exitCode = ret2.Item1;
            string output = ret2.Item2;

            Check(exitCode == 0);
            Check(output.Contains("Solution count = 92"));
        }

        public void BubbleSort()
        {
            string src = @"

/* Function to perform bubble sort */
void bubbleSort(int arr[], int n) {
    int i;
    int j;
    int temp;
    for (i = 0; i < n - 1; i++) {
        for (j = 0; j < n - i - 1; j++) {
            if (arr[j] > arr[j + 1]) {
                /* Swap arr[j] and arr[j + 1] */
                temp = arr[j];
                arr[j] = arr[j + 1];
                arr[j + 1] = temp;
            }
        }
    }
}

void printArray(int arr[], int n) {
    int i;
    for (i = 0; i < n; i++) {
        printf(""%d,"", arr[i]);
    }
    printf(""\n"");
}

int main() {
    int arr[7];
    arr[0] = 64;
    arr[1] = 34;
    arr[2] = 25;
    arr[3] = 12;
    arr[4] = 22;
    arr[5] = 11;
    arr[6] = 90;

    printf(""Original array:"");
    printArray(arr, 7);

    bubbleSort(arr, 7);

    printf(""Sorted array:"");
    printArray(arr, 7);

    return 0;
}

";
            Compiler.GenerateAsm(src, "test.s");
            Tuple<int, string> ret2 = CompileAndRun2("test.s", "test.exe");
            int exitCode = ret2.Item1;
            string output = ret2.Item2;

            Check(exitCode == 0);
            Check(output.Contains("Sorted array:11,12,22,25,34,64,90"));
        }

        public void BinarySearch()
        {
            string src = @"

int binarySearch(int arr[], int size, int target) {
    int low = 0;
    int high = size - 1;
    int mid;

    while (low <= high) {
        mid = low + (high - low) / 2;

        if (arr[mid] == target) 
            return mid;        
        else if (arr[mid] < target) 
            low = mid + 1;        
        else 
            high = mid - 1;
    }
    return 10000;
}

int main() {
    int a[8];
    int target = 7;
    int result = 10000;

    a[0] = 1;
    a[1] = 3; 
    a[2] = 5; 
    a[3] = 7; 
    a[4] = 9; 
    a[5] = 11; 
    a[6] = 13;
    a[7] = 15;

    result = binarySearch(a, 8, target);

    if (result != 10000)
        printf(""Element found at index:%d\n"", result);
    else
        printf(""Element not found in the array.\n"");
    

    return 0;
}


";
            Compiler.GenerateAsm(src, "test.s");
            Tuple<int, string> ret2 = CompileAndRun2("test.s", "test.exe");
            int exitCode = ret2.Item1;
            string output = ret2.Item2;

            Check(exitCode == 0);
            Check(output.Contains("Element found at index:3"));
        }


        public void LCS()
        {
            string src = @"

void lcs(char str1[], char str2[], int m, int n) {
    int dp[10][10];
    int lcs_length = 0;
    char lcs_str[10];
    int i;
    int j;
    int k;
    int index;

    for (i = 0; i <= m; i++)
        for (j = 0; j <= n; j++)
            if (i == 0 || j == 0)
                dp[i][j] = 0;
            else if (str1[i - 1] == str2[j - 1])
                dp[i][j] = dp[i - 1][j - 1] + 1;
            else if (dp[i - 1][j] > dp[i][j - 1])
                dp[i][j] = dp[i - 1][j];
            else
                dp[i][j] = dp[i][j - 1];

    lcs_length = dp[m][n];
    printf(""Length of Longest Common Subsequence of %s, %s : % d\n"", str1, str2, lcs_length);

    lcs_str[lcs_length] = '\0';

    i = m;
    j = n;
    index = lcs_length - 1;
    while(i > 0 && j > 0)
        if (str1[i - 1] == str2[j - 1]) {
            lcs_str[index] = str1[i - 1];
            i--;
            j--;
            index--;
        }
        else if (dp[i - 1][j] > dp[i][j - 1])
            i--;
        else
            j--;

    printf(""Longest Common Subsequence: %s\n"", lcs_str);
}

int main() {
    char *str1 = ""AGGTABWZ"";
    char *str2 = ""GXTXAYBYZ"";
    int m = strlen(str1);
    int n = strlen(str2);
    lcs(str1, str2, m, n);
    return 0;
}


";
            Compiler.GenerateAsm(src, "test.s");
            Tuple<int, string> ret2 = CompileAndRun2("test.s", "test.exe");
            int exitCode = ret2.Item1;
            string output = ret2.Item2;

            Check(exitCode == 0);
            Check(output.Contains("Longest Common Subsequence: GTABZ"));
        }

        public void MagicNumber()
        {
            string src = @"

int magicSquare[15][15];

void generateMagicSquare(int n) {
    int i;
    int j;
    int new_i;
    int new_j;

    int num = 1;
    i = 0;
    j = n / 2;

    while (num <= n * n) {
        magicSquare[i][j] = num;
        num++;

        new_i = i - 1;
        new_j = j + 1;

        if (new_i < 0) {
            new_i = n - 1;
        }
        if (new_j >= n) {
            new_j = 0;
        }

        if (magicSquare[new_i][new_j] != 0) {
            new_i = i + 1;
            if (new_i >= n) 
                new_i = 0;
            new_j = j;
        }

        i = new_i;
        j = new_j;
    }

    printf(""Magic Square of size %d:\n"", n);
    for (i = 0; i < n; i++) {
        for (j = 0; j < n; j++) {
            printf(""%4d"", magicSquare[i][j]);
        }
        printf(""\n"");
    }
}

int main() {
    generateMagicSquare(7);
    return 0;
}
";
            Compiler.GenerateAsm(src, "test.s");
            Tuple<int, string> ret2 = CompileAndRun2("test.s", "test.exe");
            int exitCode = ret2.Item1;
            string output = ret2.Item2;

            Check(exitCode == 0);
            Check(output.Contains("Magic Square of size 7:"));
            Check(output.Contains("30  39  48   1  10  19  28"));
            Check(output.Contains("22  31  40  49   2  11  20"));
        }

        public void JobAssignment()
        {
            string src = @"
struct Project {
    int id;
    char name[50];
};

struct Department {
    int id;
    char name[50];
};

struct Employee {
    int id;
    char name[50];
    struct Department department;
    struct Project projects[5];
    int project_count;
};

void print_project(struct Project project) {
    printf(""Project ID: %d, Name: %s\n"", project.id, project.name);
}

void print_department(struct Department department) {
    printf(""Department ID: %d, Name: %s\n"", department.id, department.name);
}

void print_employee(struct Employee employee) {
    int i;
    printf(""Employee ID: %d, Name: %s\n"", employee.id, employee.name);
    printf(""Department: "");
    print_department(employee.department);
    printf(""Projects:\n"");
    for (i = 0; i < employee.project_count; i++) {
        print_project(employee.projects[i]);
    }
}

int main() {
    struct Department dept1;
    struct Department dept2;
    struct Employee employees[10];

    dept1.id = 1;
    strcpy(dept1.name, ""Engineering"");
    dept2.id = 2;
    strcpy(dept2.name, ""Marketing"");

    struct Project proj1; 
    struct Project proj2; 
    struct Project proj3;
    proj1.id = 101;
    strcpy(proj1.name, ""AI Development"");
    proj2.id = 102;
    strcpy(proj2.name, ""Website Redesign"");
    proj3.id = 103;
    strcpy(proj3.name, ""Market Analysis"");

    

    employees[0].id = 1;
    strcpy(employees[0].name, ""Alice"");
    employees[0].department = dept1;
    employees[0].projects[0] = proj1;
    employees[0].projects[1] = proj2;
    employees[0].project_count = 2;

    employees[1].id = 2;
    strcpy(employees[1].name, ""Bob"");
    employees[1].department = dept2;
    employees[1].projects[0] = proj3;
    employees[1].project_count = 1;

    int i;
    for (i = 0; i < 2; i++) {
        print_employee(employees[i]);
        printf(""\n"");
    }

    return 0;
}
";
            Compiler.GenerateAsm(src, "test.s");
            Tuple<int, string> ret2 = CompileAndRun2("test.s", "test.exe");
            int exitCode = ret2.Item1;
            string output = ret2.Item2;

            Check(exitCode == 0);
            Check(output.Contains("Employee ID: 1, Name: Alice"));
            Check(output.Contains("Department: Department ID: 1, Name: Engineering"));
        }

        public void ReverseLinkedList()
        {
            string src = @"
struct Node {
    int value;
    struct Node* next;
};

void add_node(struct Node** head, int value) {
    struct Node* new_node = malloc(20);
    new_node->value = value;
    new_node->next = *head;
    *head = new_node;
}

void print_list(struct Node* head) {
    struct Node* current = head;
    while (current != 0) {
        printf(""%d -> "", current->value);
        current = current->next;
    }
    printf(""NULL\n"");
}

void reverse_list(struct Node** head) {
    struct Node* prev = 0;
    struct Node* current = *head;
    struct Node* next = 0;

    while (current != 0) {
        next = current->next;
        current->next = prev; 
        prev = current; 
        current = next; 
    }

    *head = prev; 
}

void free_list(struct Node** head) {
    struct Node* current = *head;
    while (current != 0) {
        struct Node* temp = current;
        current = current->next;
    }
}

int main() {
    struct Node* head = 0;

    add_node(&head, 10);
    add_node(&head, 20);
    add_node(&head, 30);
    add_node(&head, 40);
    add_node(&head, 50);

    printf(""Original list: "");
    print_list(head);

    reverse_list(&head);

    printf(""Reversed list: "");
    print_list(head);

    return 0;
}
";
            Compiler.GenerateAsm(src, "test.s");
            Tuple<int, string> ret2 = CompileAndRun2("test.s", "test.exe");
            int exitCode = ret2.Item1;
            string output = ret2.Item2;

            Check(exitCode == 0);
            Check(output.Contains("Original list: 50 -> 40 -> 30 -> 20 -> 10 -> NULL"));
            Check(output.Contains("Reversed list: 10 -> 20 -> 30 -> 40 -> 50 -> NULL"));
        }

        public static void RunAllUt()
        {
            MainUt mainUt = new MainUt();

            mainUt.adhoc();

            mainUt.Ut1();
            mainUt.Ut2();
            mainUt.Ut3();
            mainUt.Ut4();
            mainUt.Ut5();
            mainUt.Ut6();
            mainUt.Ut7();
            mainUt.Ut8();
            mainUt.Ut9();
            mainUt.Ut10();
            mainUt.Ut15();

            mainUt.boolean_expression_1();
            mainUt.boolean_expression_2();
            mainUt.boolean_expression_3();
            mainUt.boolean_expression_4();
            mainUt.boolean_expression_5();
            mainUt.boolean_expression_6();
            mainUt.boolean_expression_7();

            mainUt.struct_1();
            mainUt.struct_2();
            mainUt.struct_3();
            mainUt.struct_4();
            mainUt.struct_5();
            mainUt.struct_6();
            mainUt.struct_7();
            mainUt.struct_8();
            mainUt.struct_9();
            mainUt.struct_10();

            mainUt.if_1();
            mainUt.if_2();
            mainUt.if_3();
            mainUt.if_4();
            mainUt.if_5();
            mainUt.if_6();
            mainUt.if_7();

            mainUt.for_1();
            mainUt.for_2();
            mainUt.for_3();
            mainUt.for_4();
            mainUt.for_5();
            mainUt.for_6();
            mainUt.for_7();

            mainUt.call_function_1();
            mainUt.call_function_2();
            mainUt.call_function_3();
            mainUt.call_function_4();
            mainUt.call_function_5();
            mainUt.call_function_6();
            mainUt.call_function_7();

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

            mainUt.call_c_function_1();
            mainUt.call_c_function_2();
            mainUt.call_c_function_3();
            mainUt.call_c_function_4();
            mainUt.call_c_function_5();

            mainUt.pointer_1();
            mainUt.pointer_2();
            mainUt.pointer_3();
            mainUt.pointer_4();
            mainUt.pointer_5();
            mainUt.pointer_6();

            mainUt.comment_1();
            mainUt.comment_2();

            mainUt.EightQueen();
            mainUt.BubbleSort();
            mainUt.BinarySearch();
            mainUt.LCS();
            mainUt.MagicNumber();
            mainUt.JobAssignment();
            mainUt.ReverseLinkedList();

        }

        public static void Ut()
        {
            RunAllUt();
        }
    }
}
