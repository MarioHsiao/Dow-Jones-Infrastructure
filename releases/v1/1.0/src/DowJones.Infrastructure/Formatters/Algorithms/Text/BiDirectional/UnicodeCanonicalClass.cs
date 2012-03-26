// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Dow Jones" file="UnicodeCanonicalClass.cs">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace DowJones.Utilities.Formatters.Algorithms.Text.BiDirectional
{
    /// <summary>
    /// The different canonical classes of Unicode characters.
    /// </summary>
    public enum UnicodeCanonicalClass
    {
        /// <summary>Not Reordered</summary>
        /// <remarks>Spacing, split, enclosing, reordrant, and Tibetan subjoined.</remarks>
        NR = 0, 

        /// <summary>Overlays and interior</summary>
        OV = 1, 

        /// <summary>Nuktas (>>)</summary>
        NK = 7, 

        /// <summary>Hiragana/Katakana voicing marks</summary>
        KV = 8,

        /// <summary>Viramas - Generic term for the diacritic in many Brahmic scripts</summary>
        VR = 9, 

        #region Fixed Position Classes

        /// <summary>
        /// General class level 10.
        /// </summary>
        Class10 = 10, 

        /// <summary>
        /// General class level 11.
        /// </summary>
        Class11 = 11, 

        /// <summary>
        /// General class level 12.
        /// </summary>
        Class12 = 12, 

        /// <summary>
        /// General class level 13.
        /// </summary>
        Class13 = 13, 

        /// <summary>
        /// General class level 14.
        /// </summary>
        Class14 = 14, 

        /// <summary>
        /// General class level 15.
        /// </summary>
        Class15 = 15, 

        /// <summary>
        /// General class level 16.
        /// </summary>
        Class16 = 16, 

        /// <summary>
        /// General class level 17.
        /// </summary>
        Class17 = 17, 

        /// <summary>
        /// General class level 18.
        /// </summary>
        Class18 = 18, 

        /// <summary>
        /// General class level 19.
        /// </summary>
        Class19 = 19, 

        /// <summary>
        /// General class level 20.
        /// </summary>
        Class20 = 20, 

        /// <summary>
        /// General class level 21.
        /// </summary>
        Class21 = 21, 

        /// <summary>
        /// General class level 22.
        /// </summary>
        Class22 = 22, 

        /// <summary>
        /// General class level 23.
        /// </summary>
        Class23 = 23, 

        /// <summary>
        /// General class level 24.
        /// </summary>
        Class24 = 24, 

        /// <summary>
        /// General class level 25.
        /// </summary>
        Class25 = 25, 

        /// <summary>
        /// General class level 26.
        /// </summary>
        Class26 = 26, 

        /// <summary>
        /// General class level 27.
        /// </summary>
        Class27 = 27, 

        /// <summary>
        /// General class level 28.
        /// </summary>
        Class28 = 28, 

        /// <summary>
        /// General class level 29.
        /// </summary>
        Class29 = 29, 

        /// <summary>
        /// General class level 30.
        /// </summary>
        Class30 = 30, 

        /// <summary>
        /// General class level 31.
        /// </summary>
        Class31 = 31, 

        /// <summary>
        /// General class level 32.
        /// </summary>
        Class32 = 32, 

        /// <summary>
        /// General class level 33.
        /// </summary>
        Class33 = 33, 

        /// <summary>
        /// General class level 34.
        /// </summary>
        Class34 = 34, 

        /// <summary>
        /// General class level 35.
        /// </summary>
        Class35 = 35, 

        /// <summary>
        /// General class level 36.
        /// </summary>
        Class36 = 36, 

        /// <summary>
        /// General class level 37.
        /// </summary>
        Class37 = 37, 

        /// <summary>
        /// General class level 38.
        /// </summary>
        Class38 = 38, 

        /// <summary>
        /// General class level 39.
        /// </summary>
        Class39 = 39, 

        /// <summary>
        /// General class level 40.
        /// </summary>
        Class40 = 40, 

        /// <summary>
        /// General class level 41.
        /// </summary>
        Class41 = 41, 

        /// <summary>
        /// General class level 42.
        /// </summary>
        Class42 = 42, 

        /// <summary>
        /// General class level 43.
        /// </summary>
        Class43 = 43, 

        /// <summary>
        /// General class level 44.
        /// </summary>
        Class44 = 44, 

        /// <summary>
        /// General class level 45.
        /// </summary>
        Class45 = 45, 

        /// <summary>
        /// General class level 46.
        /// </summary>
        Class46 = 46, 

        /// <summary>
        /// General class level 47.
        /// </summary>
        Class47 = 47, 

        /// <summary>
        /// General class level 48.
        /// </summary>
        Class48 = 48, 

        /// <summary>
        /// General class level 49.
        /// </summary>
        Class49 = 49, 

        /// <summary>
        /// General class level 50.
        /// </summary>
        Class50 = 50, 

        /// <summary>
        /// General class level 51.
        /// </summary>
        Class51 = 51, 

        /// <summary>
        /// General class level 52.
        /// </summary>
        Class52 = 52, 

        /// <summary>
        /// General class level 53.
        /// </summary>
        Class53 = 53, 

        /// <summary>
        /// General class level 54.
        /// </summary>
        Class54 = 54, 

        /// <summary>
        /// General class level 55.
        /// </summary>
        Class55 = 55, 

        /// <summary>
        /// General class level 56.
        /// </summary>
        Class56 = 56, 

        /// <summary>
        /// General class level 57.
        /// </summary>
        Class57 = 57, 

        /// <summary>
        /// General class level 58.
        /// </summary>
        Class58 = 58, 

        /// <summary>
        /// General class level 59.
        /// </summary>
        Class59 = 59, 

        /// <summary>
        /// General class level 60.
        /// </summary>
        Class60 = 60, 

        /// <summary>
        /// General class level 61.
        /// </summary>
        Class61 = 61, 

        /// <summary>
        /// General class level 62.
        /// </summary>
        Class62 = 62, 

        /// <summary>
        /// General class level 63.
        /// </summary>
        Class63 = 63, 

        /// <summary>
        /// General class level 64.
        /// </summary>
        Class64 = 64, 

        /// <summary>
        /// General class level 65.
        /// </summary>
        Class65 = 65, 

        /// <summary>
        /// General class level 66.
        /// </summary>
        Class66 = 66, 

        /// <summary>
        /// General class level 67.
        /// </summary>
        Class67 = 67, 

        /// <summary>
        /// General class level 68.
        /// </summary>
        Class68 = 68, 

        /// <summary>
        /// General class level 69.
        /// </summary>
        Class69 = 69, 

        /// <summary>
        /// General class level 70.
        /// </summary>
        Class70 = 70, 

        /// <summary>
        /// General class level 71.
        /// </summary>
        Class71 = 71, 

        /// <summary>
        /// General class level 72.
        /// </summary>
        Class72 = 72, 

        /// <summary>
        /// General class level 73.
        /// </summary>
        Class73 = 73, 

        /// <summary>
        /// General class level 74.
        /// </summary>
        Class74 = 74, 

        /// <summary>
        /// General class level 75.
        /// </summary>
        Class75 = 75, 

        /// <summary>
        /// General class level 76.
        /// </summary>
        Class76 = 76, 

        /// <summary>
        /// General class level 77.
        /// </summary>
        Class77 = 77, 

        /// <summary>
        /// General class level 78.
        /// </summary>
        Class78 = 78, 

        /// <summary>
        /// General class level 79.
        /// </summary>
        Class79 = 79, 

        /// <summary>
        /// General class level 80.
        /// </summary>
        Class80 = 80, 

        /// <summary>
        /// General class level 81.
        /// </summary>
        Class81 = 81, 

        /// <summary>
        /// General class level 82.
        /// </summary>
        Class82 = 82, 

        /// <summary>
        /// General class level 83.
        /// </summary>
        Class83 = 83, 

        /// <summary>
        /// General class level 84.
        /// </summary>
        Class84 = 84, 

        /// <summary>
        /// General class level 85.
        /// </summary>
        Class85 = 85, 

        /// <summary>
        /// General class level 86.
        /// </summary>
        Class86 = 86, 

        /// <summary>
        /// General class level 87.
        /// </summary>
        Class87 = 87, 

        /// <summary>
        /// General class level 88.
        /// </summary>
        Class88 = 88, 

        /// <summary>
        /// General class level 89.
        /// </summary>
        Class89 = 89, 

        /// <summary>
        /// General class level 90.
        /// </summary>
        Class90 = 90, 

        /// <summary>
        /// General class level 91.
        /// </summary>
        Class91 = 91, 

        /// <summary>
        /// General class level 92.
        /// </summary>
        Class92 = 92, 

        /// <summary>
        /// General class level 93.
        /// </summary>
        Class93 = 93, 

        /// <summary>
        /// General class level 94.
        /// </summary>
        Class94 = 94, 

        /// <summary>
        /// General class level 95.
        /// </summary>
        Class95 = 95, 

        /// <summary>
        /// General class level 96.
        /// </summary>
        Class96 = 96, 

        /// <summary>
        /// General class level 97.
        /// </summary>
        Class97 = 97, 

        /// <summary>
        /// General class level 98.
        /// </summary>
        Class98 = 98, 

        /// <summary>
        /// General class level 99.
        /// </summary>
        Class99 = 99, 

        /// <summary>
        /// General class level 100.
        /// </summary>
        Class100 = 100, 

        /// <summary>
        /// General class level 101.
        /// </summary>
        Class101 = 101, 

        /// <summary>
        /// General class level 102.
        /// </summary>
        Class102 = 102, 

        /// <summary>
        /// General class level 103.
        /// </summary>
        Class103 = 103, 

        /// <summary>
        /// General class level 104.
        /// </summary>
        Class104 = 104, 

        /// <summary>
        /// General class level 105.
        /// </summary>
        Class105 = 105, 

        /// <summary>
        /// General class level 106.
        /// </summary>
        Class106 = 106, 

        /// <summary>
        /// General class level 107.
        /// </summary>
        Class107 = 107, 

        /// <summary>
        /// General class level 108.
        /// </summary>
        Class108 = 108, 

        /// <summary>
        /// General class level 109.
        /// </summary>
        Class109 = 109, 

        /// <summary>
        /// General class level 110.
        /// </summary>
        Class110 = 110, 

        /// <summary>
        /// General class level 111.
        /// </summary>
        Class111 = 111, 

        /// <summary>
        /// General class level 112.
        /// </summary>
        Class112 = 112, 

        /// <summary>
        /// General class level 113.
        /// </summary>
        Class113 = 113, 

        /// <summary>
        /// General class level 114.
        /// </summary>
        Class114 = 114, 

        /// <summary>
        /// General class level 115.
        /// </summary>
        Class115 = 115, 

        /// <summary>
        /// General class level 116.
        /// </summary>
        Class116 = 116, 

        /// <summary>
        /// General class level 117.
        /// </summary>
        Class117 = 117, 

        /// <summary>
        /// General class level 118.
        /// </summary>
        Class118 = 118, 

        /// <summary>
        /// General class level 119.
        /// </summary>
        Class119 = 119, 

        /// <summary>
        /// General class level 120.
        /// </summary>
        Class120 = 120, 

        /// <summary>
        /// General class level 121.
        /// </summary>
        Class121 = 121, 

        /// <summary>
        /// General class level 122.
        /// </summary>
        Class122 = 122, 

        /// <summary>
        /// General class level 123.
        /// </summary>
        Class123 = 123, 

        /// <summary>
        /// General class level 124.
        /// </summary>
        Class124 = 124, 

        /// <summary>
        /// General class level 125.
        /// </summary>
        Class125 = 125, 

        /// <summary>
        /// General class level 126.
        /// </summary>
        Class126 = 126, 

        /// <summary>
        /// General class level 127.
        /// </summary>
        Class127 = 127, 

        /// <summary>
        /// General class level 128.
        /// </summary>
        Class128 = 128, 

        /// <summary>
        /// General class level 129.
        /// </summary>
        Class129 = 129, 

        /// <summary>
        /// General class level 130.
        /// </summary>
        Class130 = 130, 

        /// <summary>
        /// General class level 131.
        /// </summary>
        Class131 = 131, 

        /// <summary>
        /// General class level 132.
        /// </summary>
        Class132 = 132, 

        /// <summary>
        /// General class level 133.
        /// </summary>
        Class133 = 133, 

        /// <summary>
        /// General class level 134.
        /// </summary>
        Class134 = 134, 

        /// <summary>
        /// General class level 135.
        /// </summary>
        Class135 = 135, 

        /// <summary>
        /// General class level 136.
        /// </summary>
        Class136 = 136, 

        /// <summary>
        /// General class level 137.
        /// </summary>
        Class137 = 137, 

        /// <summary>
        /// General class level 138.
        /// </summary>
        Class138 = 138, 

        /// <summary>
        /// General class level 139.
        /// </summary>
        Class139 = 139, 

        /// <summary>
        /// General class level 140.
        /// </summary>
        Class140 = 140, 

        /// <summary>
        /// General class level 141.
        /// </summary>
        Class141 = 141, 

        /// <summary>
        /// General class level 142.
        /// </summary>
        Class142 = 142, 

        /// <summary>
        /// General class level 143.
        /// </summary>
        Class143 = 143, 

        /// <summary>
        /// General class level 144.
        /// </summary>
        Class144 = 144, 

        /// <summary>
        /// General class level 145.
        /// </summary>
        Class145 = 145, 

        /// <summary>
        /// General class level 146.
        /// </summary>
        Class146 = 146, 

        /// <summary>
        /// General class level 147.
        /// </summary>
        Class147 = 147, 

        /// <summary>
        /// General class level 148.
        /// </summary>
        Class148 = 148, 

        /// <summary>
        /// General class level 149.
        /// </summary>
        Class149 = 149, 

        /// <summary>
        /// General class level 150.
        /// </summary>
        Class150 = 150, 

        /// <summary>
        /// General class level 151.
        /// </summary>
        Class151 = 151, 

        /// <summary>
        /// General class level 152.
        /// </summary>
        Class152 = 152, 

        /// <summary>
        /// General class level 153.
        /// </summary>
        Class153 = 153, 

        /// <summary>
        /// General class level 154.
        /// </summary>
        Class154 = 154, 

        /// <summary>
        /// General class level 155.
        /// </summary>
        Class155 = 155, 

        /// <summary>
        /// General class level 156.
        /// </summary>
        Class156 = 156, 

        /// <summary>
        /// General class level 157.
        /// </summary>
        Class157 = 157, 

        /// <summary>
        /// General class level 158.
        /// </summary>
        Class158 = 158, 

        /// <summary>
        /// General class level 159.
        /// </summary>
        Class159 = 159, 

        /// <summary>
        /// General class level 160.
        /// </summary>
        Class160 = 160, 

        /// <summary>
        /// General class level 161.
        /// </summary>
        Class161 = 161, 

        /// <summary>
        /// General class level 162.
        /// </summary>
        Class162 = 122, 

        /// <summary>
        /// General class level 163.
        /// </summary>
        Class163 = 166, 

        /// <summary>
        /// General class level 164.
        /// </summary>
        Class164 = 164, 

        /// <summary>
        /// General class level 165.
        /// </summary>
        Class165 = 165, 

        /// <summary>
        /// General class level 166.
        /// </summary>
        Class166 = 166, 

        /// <summary>
        /// General class level 167.
        /// </summary>
        Class167 = 167, 

        /// <summary>
        /// General class level 168.
        /// </summary>
        Class168 = 168, 

        /// <summary>
        /// General class level 169.
        /// </summary>
        Class169 = 169, 

        /// <summary>
        /// General class level 170.
        /// </summary>
        Class170 = 170, 

        /// <summary>
        /// General class level 171.
        /// </summary>
        Class171 = 171, 

        /// <summary>
        /// General class level 172.
        /// </summary>
        Class172 = 172, 

        /// <summary>
        /// General class level 173.
        /// </summary>
        Class173 = 173, 

        /// <summary>
        /// General class level 174.
        /// </summary>
        Class174 = 174, 

        /// <summary>
        /// General class level 175.
        /// </summary>
        Class175 = 175, 

        /// <summary>
        /// General class level 176.
        /// </summary>
        Class176 = 176, 

        /// <summary>
        /// General class level 177.
        /// </summary>
        Class177 = 177, 

        /// <summary>
        /// General class level 178.
        /// </summary>
        Class178 = 178, 

        /// <summary>
        /// General class level 179.
        /// </summary>
        Class179 = 179, 

        /// <summary>
        /// General class level 180.
        /// </summary>
        Class180 = 180, 

        /// <summary>
        /// General class level 181.
        /// </summary>
        Class181 = 181, 

        /// <summary>
        /// General class level 182.
        /// </summary>
        Class182 = 182, 

        /// <summary>
        /// General class level 183.
        /// </summary>
        Class183 = 183, 

        /// <summary>
        /// General class level 184.
        /// </summary>
        Class184 = 184, 

        /// <summary>
        /// General class level 185.
        /// </summary>
        Class185 = 185, 

        /// <summary>
        /// General class level 186.
        /// </summary>
        Class186 = 186, 

        /// <summary>
        /// General class level 187.
        /// </summary>
        Class187 = 187, 

        /// <summary>
        /// General class level 188.
        /// </summary>
        Class188 = 188, 

        /// <summary>
        /// General class level 189.
        /// </summary>
        Class189 = 189, 

        /// <summary>
        /// General class level 190.
        /// </summary>
        Class190 = 190, 

        /// <summary>
        /// General class level 191.
        /// </summary>
        Class191 = 191, 

        /// <summary>
        /// General class level 192.
        /// </summary>
        Class192 = 192, 

        /// <summary>
        /// General class level 193.
        /// </summary>
        Class193 = 193, 

        /// <summary>
        /// General class level 194.
        /// </summary>
        Class194 = 194, 

        /// <summary>
        /// General class level 195.
        /// </summary>
        Class195 = 195, 

        /// <summary>
        /// General class level 196.
        /// </summary>
        Class196 = 196, 

        /// <summary>
        /// General class level 197.
        /// </summary>
        Class197 = 197, 

        /// <summary>
        /// General class level 198.
        /// </summary>
        Class198 = 198, 

        /// <summary>
        /// General class level 199.
        /// </summary>
        Class199 = 199, 

        #endregion

        /// <summary>Attached Below Left</summary>
        ATBL = 200, 

        /// <summary>Attached Below</summary>
        ATB = 202, 

        /// <summary>Attached Below Right</summary>
        ATBR = 204, 

        /// <summary>Attached Left</summary>
        /// <remarks>Reordrant around single base character.</remarks>
        ATL = 208, 

        /// <summary>Attached Right</summary>
        ATR = 210, 

        /// <summary>Attached Above Left</summary>
        ATAL = 212, 

        /// <summary>Attached Above</summary>
        ATA = 214, 

        /// <summary>Attached Above Right</summary>
        ATAR = 216, 

        /// <summary>Below Left</summary>
        BL = 218, 

        /// <summary>Below</summary>
        B = 220, 

        /// <summary>Below Right</summary>
        BR = 222, 

        /// <summary>Left</summary>
        /// <remarks>Reordrant around single base character.</remarks>
        L = 224, 

        /// <summary>Right</summary>
        R = 226, 

        /// <summary>Above Left</summary>
        AL = 228, 

        /// <summary>Above</summary>
        A = 230, 

        /// <summary>Above Right</summary>
        AR = 232, 

        /// <summary>Double Below</summary>
        DB = 233, 

        /// <summary>Double Above</summary>
        DA = 234, 

        /// <summary>Iota Subscript</summary>
        IS = 240
    }
}