// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Dow Jones" file="NBidi.cs">
//   © 2010 Dow Jones, Inc. All rights reserved.
// </copyright>
// 
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Formatters.Algorithms.Text.BiDirectional;

namespace DowJones.Formatters.Algorithms.Text
{
    /// <summary>
    /// The main class that implements the BIDI algorithm.
    /// </summary>
    /// <remarks>
    /// Note that the BiDi algorithm does not cover reordering a visual string to logical form.
    /// Some times the very same algorithm will do the job, but in more complex situations it'll fail.
    /// The correct way is to always work in logical form in memory, and only run the BiDi algorithm to display the results on the screen.
    /// </remarks>
    /// <example>
    /// <code>
    /// string visual = NBidi.NBidi.LogicalToVisual(string logical)
    /// </code>
    /// </example>
    public static class Bidi
    {
        /// <summary>
        /// Implementation of the BIDI algorithm, as described in http://www.unicode.org/reports/tr9/tr9-17.html
        /// </summary>
        /// <param name="logicalString">
        /// The original logical-ordered string.
        /// </param>
        /// <returns>
        /// The visual representation of the string.
        /// </returns>
        public static string LogicalToVisual(string logicalString)
        {
            var pars = SplitStringToParagraphs(logicalString);
            var sb = new StringBuilder();
            foreach (var p in pars)
            {
                sb.Append(p.BidiText);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Implementation of the BIDI algorithm, as described in http://www.unicode.org/reports/tr9/tr9-17.html
        /// </summary>
        /// <param name="logicalString">
        /// The original logical-ordered string.
        /// </param>
        /// <param name="indexes">
        /// Implies where the original characters are.
        /// </param>
        /// <param name="lengths">
        /// Implies how many characters each original character occupies.
        /// </param>
        /// <returns>
        /// The visual representation of the string.
        /// </returns>
        public static string LogicalToVisual(string logicalString, out int[] indexes, out int[] lengths)
        {
            // Section 3:
            // 1. seperate text into paragraphs
            // 2. resulate each paragraph to its embeding levels of text
            // 2.1 find the first character of type L, AL, or R.
            // 3. reorder text elements

            // Section 3.3: Resolving Embedding Levels:
            // (1) determining the paragraph level.
            // (2) determining explicit embedding levels and directions.
            // (3) resolving weak types.
            // (4) resolving neutral types.
            // (5) resolving implicit embedding levels.
            var arrIndexes = new ArrayList();
            var arrLengths = new ArrayList();

            var pars = SplitStringToParagraphs(logicalString);
            var sb = new StringBuilder();
            foreach (var p in pars)
            {
                sb.Append(p.BidiText);
                arrIndexes.AddRange(p.BidiIndexes);
                arrLengths.AddRange(p.BidiIndexLengths);
            }

            indexes = (int[]) arrIndexes.ToArray(typeof(int));
            lengths = (int[]) arrLengths.ToArray(typeof(int));
            return sb.ToString();
        }

        /// <summary>
        /// The split string to paragraphs.
        /// </summary>
        /// <param name="logicalString">The logical string.</param>
        /// <returns>A array of <c>Paragraph</c></returns>
        /// <remarks>
        /// 3.3.1.P1 - Split the text into separate paragraphs.
        /// A paragraph separator is kept with the previous paragraph.
        /// Within each paragraph, apply all the other rules of this algorithm.
        /// </remarks>
        private static IEnumerable<Paragraph> SplitStringToParagraphs(string logicalString)
        {
            var ret = new ArrayList();
            int i;
            var sb = new StringBuilder();
            for (i = 0; i < logicalString.Length; ++i)
            {
                var c = logicalString[i];
                var charType = UnicodeCharacterDataResolver.GetBidiCharacterType(c);
                if (charType == BidiCharacterType.B)
                {
                    var p = new Paragraph(sb.ToString())
                                {
                                    ParagraphSeparator = c
                                };
                    ret.Add(p);
                    sb.Length = 0;
                }
                else
                {
                    sb.Append(c);
                }
            }

            if (sb.Length > 0)
            {
                // string ended without a paragraph separator
                ret.Add(new Paragraph(sb.ToString()));
            }

            return (Paragraph[]) ret.ToArray(typeof(Paragraph));
        }

        #region Nested type: CharData

        /// <summary>
        /// The char data.
        /// </summary>
        private struct CharData
        {
            /// <summary>
            /// The _char.
            /// </summary>
            public char Char;

            /// <summary>
            /// The character type.
            /// </summary>
            public BidiCharacterType CharacterType; // 0-18 => 5

            /// <summary>
            /// The element.
            /// </summary>
            public byte Element; // 0-62 => 6

            /// <summary>
            /// The index.
            /// </summary>
            public int Index;
        }

        #endregion

        #region Nested type: Paragraph

        /// <summary>
        /// The paragraph.
        /// </summary>
        private class Paragraph
        {
            /// <summary>
            /// The biditext.
            /// </summary>
            private string bidiText;

            /// <summary>
            /// The has arabic.
            /// </summary>
            private bool hasArabic;

            /// <summary>
            /// The has ns ms.
            /// </summary>
            private bool hasNSMs;

            /// <summary>
            /// The paragraph separator.
            /// </summary>
            private char paragraphSeparator = BidiChars.NotAChar;

            /// <summary>
            /// The current text.
            /// </summary>
            private string text;

            /// <summary>
            /// The text data.
            /// </summary>
            private CharData[] textData;

            /// <summary>
            /// Initializes a new instance of the <see cref="Paragraph"/> class.
            /// </summary>
            /// <param name="text">The original text.</param>
            public Paragraph(string text)
            {
                Text = text;
            }

            /// <summary>
            /// Gets BidiIndexes.
            /// </summary>
            public int[] BidiIndexes { get; private set; }

            /// <summary>
            /// Gets BidiIndexLengths.
            /// </summary>
            public int[] BidiIndexLengths { get; private set; }

            /// <summary>
            /// Sets ParagraphSeparator.
            /// </summary>
            public char ParagraphSeparator
            {
                set { paragraphSeparator = value; }
            }

            /// <summary>
            /// Gets BidiText.
            /// </summary>
            public string BidiText
            {
                get
                {
                    var ret = bidiText;
                    if (paragraphSeparator != BidiChars.NotAChar)
                    {
                        ret = string.Concat(ret, paragraphSeparator);
                    }

                    return ret;
                }
            }

            /// <summary>
            /// Gets or sets EmbeddingLevel.
            /// </summary>
            private byte EmbeddingLevel { get; set; }

            /// <summary>
            /// Sets Text.
            /// </summary>
            private string Text
            {
                set
                {
                    text = value;
                    NormalizeText();
                    RecalculateParagraphEmbeddingLevel();
                    RecalculateCharactersEmbeddingLevels();
                    RemoveBidiMarkers();
                }
            }

            /// <summary>
            /// The get pairwise composition.
            /// </summary>
            /// <param name="first">The first.</param>
            /// <param name="second">The second.</param>
            /// <returns>A char of the pairwise composition.</returns>
            private static char GetPairwiseComposition(char first, char second)
            {
                if (first < 0 || first > 0xFFFF || second < 0 || second > 0xFFFF)
                {
                    return BidiChars.NotAChar;
                }

                return UnicodeCharacterDataResolver.Compose(first + second.ToString());
            }

            /// <summary>
            /// The internal compose.
            /// </summary>
            /// <param name="target">The target.</param>
            /// <param name="char_lengths">The char_lengths.</param>
            private static void InternalCompose(StringBuilder target, ArrayList char_lengths)
            {
                if (target.Length == 0)
                {
                    return;
                }

                var starterPos = 0;
                var compPos = 1;
                var starterCh = target[0];

                char_lengths[starterPos] = (int)char_lengths[starterPos] + 1;

                var lastClass = UnicodeCharacterDataResolver.GetUnicodeCanonicalClass(starterCh);

                if (lastClass != UnicodeCanonicalClass.NR)
                {
                    lastClass = (UnicodeCanonicalClass)256; // fix for strings staring with a combining mark
                }

                var oldLen = target.Length;

                // Loop on the decomposed characters, combining where possible
                for (var decompPos = compPos; decompPos < target.Length; ++decompPos)
                {
                    var ch = target[decompPos];
                    var charClass = UnicodeCharacterDataResolver.GetUnicodeCanonicalClass(ch);
                    var composite = GetPairwiseComposition(starterCh, ch);
                    var composeType = UnicodeCharacterDataResolver.GetUnicodeDecompositionType(composite);

                    if (composeType == UnicodeDecompositionType.None &&
                        composite != BidiChars.NotAChar &&
                        (lastClass < charClass || lastClass == UnicodeCanonicalClass.NR))
                    {
                        target[starterPos] = composite;
                        char_lengths[starterPos] = (int)char_lengths[starterPos] + 1;

                        // we know that we will only be replacing non-supplementaries by non-supplementaries
                        // so we don't have to adjust the decompPos
                        starterCh = composite;
                    }
                    else
                    {
                        if (charClass == UnicodeCanonicalClass.NR)
                        {
                            starterPos = compPos;
                            starterCh = ch;
                        }

                        lastClass = charClass;
                        target[compPos] = ch;

                        // char_lengths[compPos] = (int)char_lengths[compPos] + 1;
                        var chkPos = compPos;
                        if ((int)char_lengths[chkPos] < 0)
                        {
                            while ((int)char_lengths[chkPos] < 0)
                            {
                                char_lengths[chkPos] = (int)char_lengths[chkPos] + 1;
                                char_lengths.Insert(compPos, 0);
                                chkPos++;
                            }
                        }
                        else
                        {
                            char_lengths[chkPos] = (int)char_lengths[chkPos] + 1;
                        }

                        if (target.Length != oldLen)
                        {
                            // MAY HAVE TO ADJUST!
                            decompPos += target.Length - oldLen;
                            oldLen = target.Length;
                        }

                        ++compPos;
                    }
                }

                target.Length = compPos;
                char_lengths.RemoveRange(compPos, char_lengths.Count - compPos);
            }

            /// <summary>
            /// The get recursive decomposition.
            /// </summary>
            /// <param name="canonical">The canonical.</param>
            /// <param name="ch">The character.</param>
            /// <param name="builder">The builder.</param>
            private static void GetRecursiveDecomposition(bool canonical, char ch, StringBuilder builder)
            {
                var decomp = UnicodeCharacterDataResolver.GetUnicodeDecompositionMapping(ch);
                if (decomp != null && !(canonical && UnicodeCharacterDataResolver.GetUnicodeDecompositionType(ch) != UnicodeDecompositionType.None))
                {
                    foreach (var t in decomp)
                    {
                        GetRecursiveDecomposition(canonical, t, builder);
                    }
                }
                else
                {
                    // if no decomp, append
                    builder.Append(ch);
                }
            }

            /// <summary>
            /// Return the strong type (L or R) corresponding to the embedding level.
            /// </summary>
            /// <param name="level">The embedding level to check.</param>
            /// <returns>A BidiCharacterType corresponding to the embedding level</returns>
            private static BidiCharacterType TypeForLevel(int level)
            {
                return ((level & 1) == 0) ? BidiCharacterType.L : BidiCharacterType.R;
            }

            /// <summary>
            /// The recalculate characters embedding levels.
            /// </summary>
            private void RecalculateCharactersEmbeddingLevels()
            {
                // 3.3.2 Explicit Levels and Directions
                // This method is implemented in such a way it handles the string in logical order,
                // rather than visual order, so it is easier to handle complex layouts. That is why
                // it is placed BEFORE ReorderString rather than AFTER it, as its number suggests.
                if (hasArabic)
                {
                    text = PerformArabicShaping(text);
                }

                textData = new CharData[text.Length];

                // X1
                var embedevel = EmbeddingLevel;
                var dos = DirectionalOverrideStatus.Neutral;
                var dosStack = new Stack();
                var elStack = new Stack();
                var idx = 0;
                for (var i = 0; i < text.Length; ++i)
                {
                    var x9Char = false;
                    var c = text[i];
                    textData[i].CharacterType = UnicodeCharacterDataResolver.GetBidiCharacterType(c);
                    textData[i].Char = c;
                    textData[i].Index = idx;
                    idx += BidiIndexLengths[i];

                    #region rules X2 - X5

                    // X2. With each RLE, compute the least greater odd embedding level.
                    // X4. With each RLO, compute the least greater odd embedding level.
                    switch (c)
                    {
                        case BidiChars.RLO:
                        case BidiChars.RLE:
                            x9Char = true;
                            if (embedevel < 60)
                            {
                                elStack.Push(embedevel);
                                dosStack.Push(dos);

                                ++embedevel;
                                embedevel |= 1;

                                dos = c == BidiChars.RLE ? DirectionalOverrideStatus.Neutral : DirectionalOverrideStatus.RTL;
                            }

                            break;
                        case BidiChars.LRO:
                        case BidiChars.LRE:
                            x9Char = true;
                            if (embedevel < 59)
                            {
                                elStack.Push(embedevel);
                                dosStack.Push(dos);

                                embedevel |= 1;
                                ++embedevel;

                                dos = c == BidiChars.LRE ? DirectionalOverrideStatus.Neutral : DirectionalOverrideStatus.LTR;
                            }
                            
                            break;
                        default:
                            if (c != BidiChars.PDF)
                            {
                                // a. Set the level of the current character to the current embedding level.
                                textData[i].Element = embedevel;

                                // b. Whenever the directional override status is not neutral,
                                // reset the current character type to the directional override status.
                                switch (dos)
                                {
                                    case DirectionalOverrideStatus.LTR:
                                        textData[i].CharacterType = BidiCharacterType.L;
                                        break;
                                    case DirectionalOverrideStatus.RTL:
                                        textData[i].CharacterType = BidiCharacterType.R;
                                        break;
                                }
                            }
                            
                            #endregion

                            #region rule X7
                        
                            // Terminating Embeddings and Overrides
                            // X7. With each PDF, determine the matching embedding or override code.
                            // If there was a valid matching code, restore (pop) the last remembered (pushed)
                            // embedding level and directional override.
                            else if (c == BidiChars.PDF)
                            {
                                x9Char = true;
                                if (elStack.Count > 0)
                                {
                                    embedevel = (byte) elStack.Pop();
                                    dos = (DirectionalOverrideStatus) dosStack.Pop();
                                }
                            }

                            break;
                    }

                    #endregion

                    // X8. All explicit directional embeddings and overrides are completely
                    // terminated at the end of each paragraph. Paragraph separators are not
                    // included in the embedding.
                    if (x9Char || textData[i].CharacterType == BidiCharacterType.BN)
                    {
                        textData[i].Element = embedevel;
                    }
                }

                // X10. The remaining rules are applied to each run of characters at the same level.
                int prevLevel = EmbeddingLevel;
                var start = 0;
                while (start < text.Length)
                {
                    #region rule X10 - run level setup

                    var level = textData[start].Element;
                    var sor = TypeForLevel(Math.Max(prevLevel, level));

                    var limit = start + 1;
                    while (limit < text.Length && textData[limit].Element == level)
                    {
                        ++limit;
                    }

                    var nextLevel = limit < text.Length ? textData[limit].Element : EmbeddingLevel;
                    var eor = TypeForLevel(Math.Max(nextLevel, level));

                    #endregion

                    ResolveWeakTypes(start, limit, sor, eor);
                    ResolveNeutralTypes(start, limit, sor, eor, level);
                    ResolveImplicitTypes(start, limit, level);

                    prevLevel = level;
                    start = limit;
                }

                ReorderString();
                FixMirroredCharacters();

                var indexes = new ArrayList();
                var lengths = new ArrayList();

                var sb = new StringBuilder();
                foreach (var cd in textData)
                {
                    sb.Append(cd.Char);
                    indexes.Add(cd.Index);
                    lengths.Add(1);
                }

                bidiText = sb.ToString();
                BidiIndexes = (int[]) indexes.ToArray(typeof(int));
            }

            /// <summary>
            /// 3.3.3 Resolving Weak Types
            /// </summary>
            /// <param name="start">The start.</param>
            /// <param name="limit">The limit.</param>
            /// <param name="sor">The start character type.</param>
            /// <param name="eor">The end character type.</param>
            private void ResolveWeakTypes(int start, int limit, BidiCharacterType sor, BidiCharacterType eor)
            {
                // TODO - all these repeating runs seems somewhat unefficient...
                // TODO - rules 2 and 7 are the same, except for minor parameter changes...

                // W1. Examine each nonspacing mark (NSM) in the level run, and change the type of the NSM to the type of the previous character. If the NSM is at the start of the level run, it will get the type of sor.
                if (hasNSMs)
                {
                    var preceedingCharacterType = sor;
                    for (var i = start; i < limit; ++i)
                    {
                        var t = textData[i].CharacterType;
                        if (t == BidiCharacterType.NSM)
                        {
                            textData[i].CharacterType = preceedingCharacterType;
                        }
                        else
                        {
                            preceedingCharacterType = t;
                        }
                    }
                }

                #region rule W2

                // W2. Search backward from each instance of a European number until the first strong type (R, L, AL, or sor) is found. If an AL is found, change the type of the European number to Arabic number.
                var ruleW2 = BidiCharacterType.EN;
                for (var i = start; i < limit; ++i)
                {
                    switch (textData[i].CharacterType)
                    {
                        case BidiCharacterType.R:
                        case BidiCharacterType.L:
                            ruleW2 = BidiCharacterType.EN;
                            break;
                        case BidiCharacterType.AL:
                            ruleW2 = BidiCharacterType.AN;
                            break;
                        case BidiCharacterType.EN:
                            textData[i].CharacterType = ruleW2;
                            break;
                    }
                }

                #endregion

                #region rule #3

                // W3. Change all ALs to R.
                if (hasArabic)
                {
                    for (var i = start; i < limit; ++i)
                    {
                        if (textData[i].CharacterType == BidiCharacterType.AL)
                        {
                            textData[i].CharacterType = BidiCharacterType.R;
                        }
                    }
                }

                #endregion

                #region rule W4

                // W4. A single European separator between two European numbers changes to a European number. A single common separator between two numbers of the same type changes to that type.

                // Since there must be values on both sides for this rule to have an
                // effect, the scan skips the first and last value.
                // Although the scan proceeds left to right, and changes the type values
                // in a way that would appear to affect the computations later in the scan,
                // there is actually no problem.  A change in the current value can only
                // affect the value to its immediate right, and only affect it if it is
                // ES or CS.  But the current value can only change if the value to its
                // right is not ES or CS.  Thus either the current value will not change,
                // or its change will have no effect on the remainder of the analysis.
                for (var i = start + 1; i < limit - 1; ++i)
                {
                    if (textData[i].CharacterType != BidiCharacterType.ES && textData[i].CharacterType != BidiCharacterType.CS)
                    {
                        continue;
                    }

                    var prevSepType = textData[i - 1].CharacterType;
                    var succSepType = textData[i + 1].CharacterType;
                    if (prevSepType == BidiCharacterType.EN && succSepType == BidiCharacterType.EN)
                    {
                        textData[i].CharacterType = BidiCharacterType.EN;
                    }
                    else if (textData[i].CharacterType == BidiCharacterType.CS && prevSepType == BidiCharacterType.AN && succSepType == BidiCharacterType.AN)
                    {
                        textData[i].CharacterType = BidiCharacterType.AN;
                    }
                }

                #endregion

                #region rule W5

                // W5. A sequence of European terminators adjacent to European numbers changes to all European numbers.
                for (var i = start; i < limit; ++i)
                {
                    if (textData[i].CharacterType != BidiCharacterType.ET)
                    {
                        continue;
                    }

                    // locate end of sequence
                    var runstart = i;
                    var runlimit = FindRunLimit(runstart, limit, new[] { BidiCharacterType.ET });

                    // check values at ends of sequence
                    var t = runstart == start ? sor : textData[runstart - 1].CharacterType;

                    if (t != BidiCharacterType.EN)
                    {
                        t = runlimit == limit ? eor : textData[runlimit].CharacterType;
                    }

                    if (t == BidiCharacterType.EN)
                    {
                        SetTypes(runstart, runlimit, BidiCharacterType.EN);
                    }

                    // continue at end of sequence
                    i = runlimit;
                }

                #endregion

                #region rule W6

                // W6. Otherwise, separators and terminators change to Other Neutral.
                for (var i = start; i < limit; ++i)
                {
                    var t = textData[i].CharacterType;
                    if (t == BidiCharacterType.ES || t == BidiCharacterType.ET || t == BidiCharacterType.CS)
                    {
                        textData[i].CharacterType = BidiCharacterType.ON;
                    }
                }

                #endregion

                #region rule W7

                // W7. Search backward from each instance of a European number until the first strong type (R, L, or sor) is found.
                // If an L is found, then change the type of the European number to L.
                var ruleW7 = sor == BidiCharacterType.L ? BidiCharacterType.L : BidiCharacterType.EN;
                for (var i = start; i < limit; ++i)
                {
                    switch (textData[i].CharacterType)
                    {
                        case BidiCharacterType.R:
                            ruleW7 = BidiCharacterType.EN;
                            break;
                        case BidiCharacterType.L:
                            ruleW7 = BidiCharacterType.L;
                            break;
                        case BidiCharacterType.EN:
                            textData[i].CharacterType = ruleW7;
                            break;
                    }
                }

                #endregion
            }

            /// <summary>
            /// The recalculate paragraph embedding level.
            /// </summary>
            private void RecalculateParagraphEmbeddingLevel()
            {
                // 3.3.1 The Paragraph Level
                // P2 - In each paragraph, find the first character of type L, AL, or R.
                // P3 - If a character is found in P2 and it is of type AL or R, then
                // set the paragraph embedding level to one; otherwise, set it to zero.
                foreach (var charType in text.Select(c => UnicodeCharacterDataResolver.GetBidiCharacterType(c)))
                {
                    if (charType == BidiCharacterType.R || charType == BidiCharacterType.AL)
                    {
                        EmbeddingLevel = 1;
                        break;
                    }

                    if (charType == BidiCharacterType.L)
                    {
                        break;
                    }
                }
            }

            /// <summary>
            /// The normalize text.
            /// </summary>
            private void NormalizeText()
            {
                var charLengths = new ArrayList();
                var sb = InternalDecompose(charLengths);
                InternalCompose(sb, charLengths);
                BidiIndexLengths = (int[])charLengths.ToArray(typeof(int));
                text = sb.ToString();
            }

            /// <summary>
            /// The remove bidi markers.
            /// </summary>
            private void RemoveBidiMarkers()
            {
                const string ControlChars = "\u200F\u202B\u202E\u200E\u202A\u202D\u202C";

                var sb = new StringBuilder(bidiText);
                var idxArr = new ArrayList(BidiIndexes);
                var lenArr = new ArrayList(BidiIndexLengths);

                var i = 0;
                while (i < sb.Length)
                {
                    if (ControlChars.Contains(sb[i].ToString()))
                    {
                        sb.Remove(i, 1);
                        idxArr.RemoveAt(i);
                        lenArr.RemoveAt(i);
                    }
                    else
                    {
                        ++i;
                    }
                }

                bidiText = sb.ToString();
                BidiIndexes = (int[])idxArr.ToArray(typeof(int));
                BidiIndexLengths = (int[])lenArr.ToArray(typeof(int));
            }

            /// <summary>
            /// 3.3.4 Resolving Neutral Types
            /// </summary>
            /// <param name="start">The start.</param>
            /// <param name="limit">The limit.</param>
            /// <param name="sor">The start order.</param>
            /// <param name="eor">The end order.</param>
            /// <param name="level">The level.</param>
            private void ResolveNeutralTypes(int start, int limit, BidiCharacterType sor, BidiCharacterType eor, int level)
            {
                // N1. A sequence of neutrals takes the direction of the surrounding strong text if the text on both sides has the same direction. 
                // European and Arabic numbers act as if they were R in terms of their influence on neutrals.
                // Start-of-level-run (sor) and end-of-level-run (eor) are used at level run boundaries.
                // N2. Any remaining neutrals take the embedding direction.
                for (var i = start; i < limit; ++i)
                {
                    var t = textData[i].CharacterType;
                    if (((t != BidiCharacterType.WS && t != BidiCharacterType.ON) && t != BidiCharacterType.B) && t != BidiCharacterType.S)
                    {
                        continue;
                    }

                    // find bounds of run of neutrals
                    var runstart = i;
                    var runlimit = FindRunLimit(runstart, limit, new[] { BidiCharacterType.B, BidiCharacterType.S, BidiCharacterType.WS, BidiCharacterType.ON });

                    // determine effective types at ends of run
                    BidiCharacterType leadingType;
                    BidiCharacterType trailingType;

                    if (runstart == start)
                    {
                        leadingType = sor;
                    }
                    else
                    {
                        leadingType = textData[runstart - 1].CharacterType;
                        if (leadingType == BidiCharacterType.AN || leadingType == BidiCharacterType.EN)
                        {
                            leadingType = BidiCharacterType.R;
                        }
                    }

                    if (runlimit == limit)
                    {
                        trailingType = eor;
                    }
                    else
                    {
                        trailingType = textData[runlimit].CharacterType;
                        if (trailingType == BidiCharacterType.AN || trailingType == BidiCharacterType.EN)
                        {
                            trailingType = BidiCharacterType.R;
                        }
                    }

                    var resolvedType = leadingType == trailingType ? leadingType : TypeForLevel(level);

                    SetTypes(runstart, runlimit, resolvedType);
                        
                    // skip over run of (former) neutrals
                    i = runlimit;
                }
            }

            /// <summary>
            /// 3.3.5 Resolving Implicit Levels
            /// </summary>
            /// <param name="start">The start.</param>
            /// <param name="limit">The limit.</param>
            /// <param name="level">The level.</param>
            private void ResolveImplicitTypes(int start, int limit, int level)
            {
                // I1. For all characters with an even (left-to-right) embedding direction, those of type R go up one level and those of type AN or EN go up two levels.
                // I2. For all characters with an odd (right-to-left) embedding direction, those of type L, EN or AN go up one level.
                if ((level & 1) == 0)
                {
                    // even level
                    for (var i = start; i < limit; ++i)
                    {
                        var t = textData[i].CharacterType;

                        // Rule I1.
                        switch (t)
                        {
                            case BidiCharacterType.R:
                                textData[i].Element += 1;
                                break;
                            case BidiCharacterType.EN:
                            case BidiCharacterType.AN:
                                textData[i].Element += 2;
                                break;
                        }
                    }
                }
                else
                {
                    // odd level
                    for (var i = start; i < limit; ++i)
                    {
                        var t = textData[i].CharacterType;
                        
                        // Rule I2.
                        if (t == BidiCharacterType.L || t == BidiCharacterType.AN || t == BidiCharacterType.EN)
                        {
                            textData[i].Element += 1;
                        }
                    }
                }
            }

            /// <summary>
            /// 3.4 Reordering Resolved Levels
            /// </summary>
            private void ReorderString()
            {
                // L1. On each line, reset the embedding level of the following characters to the paragraph embedding level:
                // 1. Segment separators,
                // 2. Paragraph separators,
                // 3. Any sequence of whitespace characters preceding a segment separator or paragraph separator, and
                // 4. Any sequence of white space characters at the end of the line.
                var levelOneStart = 0;
                for (var i = 0; i < textData.Length; ++i)
                {
                    if (textData[i].CharacterType == BidiCharacterType.S || textData[i].CharacterType == BidiCharacterType.B)
                    {
                        for (var j = levelOneStart; j <= i; ++j)
                        {
                            textData[j].Element = EmbeddingLevel;
                        }
                    }

                    if (textData[i].CharacterType != BidiCharacterType.WS)
                    {
                        levelOneStart = i + 1;
                    }
                }

                for (var j = levelOneStart; j < textData.Length; ++j)
                {
                    textData[j].Element = EmbeddingLevel;
                }

                // L2. From the highest level found in the text to the lowest odd level on each
                // line, including intermediate levels not actually present in the text,
                // reverse any contiguous sequence of characters that are at that level or
                // higher.
                byte highest = 0;
                byte lowestOdd = 63;
                foreach (var cd in textData)
                {
                    if (cd.Element > highest)
                    {
                        highest = cd.Element;
                    }

                    if ((cd.Element & 1) == 1 && cd.Element < lowestOdd)
                    {
                        lowestOdd = cd.Element;
                    }
                }

                for (var el = highest; el >= lowestOdd; --el)
                {
                    for (var i = 0; i < textData.Length; ++i)
                    {
                        if (textData[i].Element < el)
                        {
                            continue;
                        }

                        // find range of text at or above this level
                        var levelTwoStart = i;
                        var limit = i + 1;
                        while (limit < textData.Length && textData[limit].Element >= el)
                        {
                            ++limit;
                        }

                        // reverse run
                        for (int j = levelTwoStart, k = limit - 1; j < k; ++j, --k)
                        {
                            var tempCd = textData[j];
                            textData[j] = textData[k];
                            textData[k] = tempCd;
                        }

                        // skip to end of level run
                        i = limit;
                    }
                }

                // TODO - L3. Combining marks applied to a right-to-left base character will at this point precede their base 
                // character. If the rendering engine expects them to follow the base characters in the final display process,
                // then the ordering of the marks and the base character must be reversed.
            }

            /// <summary>
            /// L4. A character is depicted by a mirrored glyph if and only if (a) the resolved directionality of that character is R, and (b) the Bidi_Mirrored property value of that character is true.
            /// </summary>
            private void FixMirroredCharacters()
            {
                for (var i = 0; i < textData.Length; ++i)
                {
                    if ((textData[i].Element & 1) == 1)
                    {
                        textData[i].Char = BidiCharacterMirrorResolver.GetBidiCharacterMirror(textData[i].Char);
                    }
                }
            }

            /// <summary>
            /// 3.5 Shaping
            /// Implements rules R1-R7 and rules L1-L3 of section 8.2 (Arabic) of the Unicode standard.
            /// </summary>
            /// <param name="originalTextString">The original text.</param>
            /// <returns>The perform arabic shaping.</returns>
            private string PerformArabicShaping(string originalTextString)
            {
                var lastJt = ArabicShapeJoiningType.U;
                var lastForm = LetterForm.Isolated;
                var lastPos = 0;
                var letterForms = new LetterForm[originalTextString.Length];

                for (var currPos = 0; currPos < originalTextString.Length; ++currPos)
                {
                    var ch = originalTextString[currPos];

                    // string chStr = ((int)ch).ToString("X4");
                    var jt = UnicodeArabicShapingResolver.GetArabicShapeJoiningType(ch);
                    if ((jt == ArabicShapeJoiningType.R ||
                         jt == ArabicShapeJoiningType.D ||
                         jt == ArabicShapeJoiningType.C) &&
                        (lastJt == ArabicShapeJoiningType.L ||
                         lastJt == ArabicShapeJoiningType.D ||
                         lastJt == ArabicShapeJoiningType.C))
                    {
                        if (lastForm == LetterForm.Isolated && (lastJt == ArabicShapeJoiningType.D ||
                                                                 lastJt == ArabicShapeJoiningType.L))
                        {
                            letterForms[lastPos] = LetterForm.Initial;
                        }
                        else if (lastForm == LetterForm.Final && lastJt == ArabicShapeJoiningType.D)
                        {
                            letterForms[lastPos] = LetterForm.Medial;
                        }

                        letterForms[currPos] = LetterForm.Final;
                        lastForm = LetterForm.Final;
                        lastJt = jt;
                        lastPos = currPos;
                        //// lastChar = ch;
                    }
                    else if (jt != ArabicShapeJoiningType.T)
                    {
                        letterForms[currPos] = LetterForm.Isolated;
                        lastForm = LetterForm.Isolated;
                        lastJt = jt;
                        lastPos = currPos;
                        //// lastChar = ch;
                    }
                    else
                    {
                        letterForms[currPos] = LetterForm.Isolated;
                    }
                }

                var lastChar = BidiChars.NotAChar;
                lastPos = 0;
                var insertPos = 0;

                var sb = new StringBuilder();

                var lenArr = new ArrayList(BidiIndexLengths);

                for (var currPos = 0; currPos < originalTextString.Length; ++currPos)
                {
                    var ch = originalTextString[currPos];

                    // string chStr = ((int)ch).ToString("X4");
                    var jt = UnicodeArabicShapingResolver.GetArabicShapeJoiningType(ch);

                    if (lastChar == BidiChars.ARABIC_LAM &&
                        ch != BidiChars.ARABIC_ALEF &&
                        ch != BidiChars.ARABIC_ALEF_MADDA_ABOVE &&
                        ch != BidiChars.ARABIC_ALEF_HAMZA_ABOVE &&
                        ch != BidiChars.ARABIC_ALEF_HAMZA_BELOW &&
                        jt != ArabicShapeJoiningType.T)
                    {
                        lastChar = BidiChars.NotAChar;
                    }
                    else if (ch == BidiChars.ARABIC_LAM)
                    {
                        lastChar = ch;
                        lastPos = currPos;
                        insertPos = sb.Length;
                    }

                    if (lastChar == BidiChars.ARABIC_LAM)
                    {
                        switch (letterForms[lastPos])
                        {
                            case LetterForm.Medial:
                                switch (ch)
                                {
                                    case BidiChars.ARABIC_ALEF:
                                        sb[insertPos] = BidiChars.ARABIC_LAM_ALEF_FINAL;
                                        lenArr.RemoveAt(insertPos);
                                        continue;

                                    case BidiChars.ARABIC_ALEF_MADDA_ABOVE:
                                        sb[insertPos] = BidiChars.ARABIC_LAM_ALEF_MADDA_ABOVE_FINAL;
                                        lenArr.RemoveAt(insertPos);
                                        lenArr[insertPos] = (int) lenArr[insertPos] + 1;
                                        continue;

                                    case BidiChars.ARABIC_ALEF_HAMZA_ABOVE:
                                        sb[insertPos] = BidiChars.ARABIC_LAM_ALEF_HAMZA_ABOVE_FINAL;
                                        lenArr.RemoveAt(insertPos);
                                        continue;

                                    case BidiChars.ARABIC_ALEF_HAMZA_BELOW:
                                        sb[insertPos] = BidiChars.ARABIC_LAM_ALEF_HAMZA_BELOW_FINAL;
                                        lenArr.RemoveAt(insertPos);
                                        continue;
                                }

                                break;
                            case LetterForm.Initial:
                                switch (ch)
                                {
                                    case BidiChars.ARABIC_ALEF:
                                        sb[insertPos] = BidiChars.ARABIC_LAM_ALEF_ISOLATED;
                                        lenArr.RemoveAt(insertPos);
                                        continue;

                                    case BidiChars.ARABIC_ALEF_MADDA_ABOVE:
                                        sb[insertPos] = BidiChars.ARABIC_LAM_ALEF_MADDA_ABOVE_ISOLATED;
                                        lenArr.RemoveAt(insertPos);
                                        lenArr[insertPos] = (int) lenArr[insertPos] + 1;
                                        continue;

                                    case BidiChars.ARABIC_ALEF_HAMZA_ABOVE:
                                        sb[insertPos] = BidiChars.ARABIC_LAM_ALEF_HAMZA_ABOVE_ISOLATED;
                                        lenArr.RemoveAt(insertPos);
                                        continue;

                                    case BidiChars.ARABIC_ALEF_HAMZA_BELOW:
                                        sb[insertPos] = BidiChars.ARABIC_LAM_ALEF_HAMZA_BELOW_ISOLATED;
                                        lenArr.RemoveAt(insertPos);
                                        continue;
                                }

                                break;
                        }
                    }

                    sb.Append(UnicodeArabicShapingResolver.GetArabicCharacterByLetterForm(ch, letterForms[currPos]));
                }

                BidiIndexLengths = (int[]) lenArr.ToArray(typeof(int));

                return sb.ToString();
            }

            /// <summary>
            /// The internal decompose.
            /// </summary>
            /// <param name="charLengths">The char_lengths.</param>
            /// <returns>A StringBuilder Class</returns>
            private StringBuilder InternalDecompose(IList charLengths)
            {
                var target = new StringBuilder();
                var buffer = new StringBuilder();

                hasArabic = false;
                hasNSMs = false;

                foreach (var t in text)
                {
                    var ct = UnicodeCharacterDataResolver.GetBidiCharacterType(t);
                    hasArabic |= (ct == BidiCharacterType.AL) || (ct == BidiCharacterType.AN);
                    hasNSMs |= ct == BidiCharacterType.NSM;

                    buffer.Length = 0;
                    GetRecursiveDecomposition(false, t, buffer);
                    charLengths.Add(1 - buffer.Length);

                    // add all of the characters in the decomposition.
                    // (may be just the original character, if there was
                    // no decomposition mapping)
                    for (var j = 0; j < buffer.Length; ++j)
                    {
                        var ch = buffer[j];
                        var charClass = UnicodeCharacterDataResolver.GetUnicodeCanonicalClass(ch);
                        var k = target.Length; // insertion point
                        if (charClass != UnicodeCanonicalClass.NR)
                        {
                            // bubble-sort combining marks as necessary
                            for (; k > 0; --k)
                            {
                                var ch2 = target[k - 1];
                                if (UnicodeCharacterDataResolver.GetUnicodeCanonicalClass(ch2) <= charClass)
                                {
                                    break;
                                }
                            }
                        }

                        target.Insert(k, ch);
                    }
                }

                return target;
            }

            /// <summary>
            /// Return the limit of the run, starting at index, that includes only resultTypes in validSet.
            /// This checks the value at index, and will return index if that value is not in validSet.
            /// </summary>
            /// <param name="index">The index.</param>
            /// <param name="limit">The limit.</param>
            /// <param name="validSet">The valid set.</param>
            /// <returns>The find run limit.</returns>
            private int FindRunLimit(int index, int limit, IList<BidiCharacterType> validSet)
            {
                --index;
                while (++index < limit)
                {
                    var t = textData[index].CharacterType;
                    var found = false;
                    for (var i = 0; i < validSet.Count && !found; ++i)
                    {
                        if (t == validSet[i])
                        {
                            found = true;
                        }
                    }

                    if (!found)
                    {
                        return index; // didn't find a match in validSet
                    }
                }

                return limit;
            }

            /// <summary>
            /// Set resultTypes from start up to (but not including) limit to newType.
            /// </summary>
            /// <param name="start">The start.</param>
            /// <param name="limit">The limit.</param>
            /// <param name="newType">The new type.</param>
            private void SetTypes(int start, int limit, BidiCharacterType newType)
            {
                for (var i = start; i < limit; ++i)
                {
                    textData[i].CharacterType = newType;
                }
            }
        }

        #endregion
    }
}