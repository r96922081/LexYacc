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
    printf("Length of Longest Common Subsequence of %s, %s : % d\n", str1, str2, lcs_length);

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

    printf("Longest Common Subsequence: %s\n", lcs_str);
}

int main() {
    char str1[10];
    strcpy_s(str1, 10, "AGGTABWZ");
    char str2[10];
    strcpy_s(str2, 10, "GXTXAYBYZ");
    int m = strlen(str1);
    int n = strlen(str2);
    lcs(str1, str2, m, n);
    return 0;
}