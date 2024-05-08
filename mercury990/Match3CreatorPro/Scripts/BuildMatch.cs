using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Match3Engine
{
    /// <summary>Enable matching on the grid.</summary>
    public class BuildMatch : MonoBehaviour
    {

        #region Presets
        /// <summary>Class of custom matching.</summary>
        class MatchSet
        {
                public string Name;
                public Vector2Int[] Sequence;
                public string[] Sequence_ElementNumber;
                public int Sequence_Confirmed;

        }

        static int Sequence_Count = 0;
        static List<MatchSet> Sequences = new List<MatchSet>();

        static List<bool[,]> AnyElementMatches = new List<bool[,]>();
        static List<int[,]>  SpecificElementMatches = new List<int[,]>();

        //Default sequence to check for matches
        static Vector2Int[] DefaultSeq1()
            {

                Vector2Int[] blocks = new Vector2Int[GridSetting.Columns];

                for (int i = 1; i < GridSetting.Columns + 1; i++)
                {
                    blocks[i - 1] = new Vector2Int(0, i);
                }

                return blocks;
            }
        static Vector2Int[] DefaultSeq2()
            {

                Vector2Int[] blocks = new Vector2Int[GridSetting.Rows];

                for (int i = 1; i < GridSetting.Rows + 1; i++)
                {
                    blocks[i - 1] = new Vector2Int(i, 0);
                }

                return blocks;
            }

        static string[] DefaultElements1()
            {
                string[] E = new string[GridSetting.Columns];

                for (int i = 1; i < GridSetting.Columns + 1; i++)
                {
                    E[i - 1] = "same";
                }

                return E;
            }
        static string[] DefaultElements2()
            {
                string[] E = new string[GridSetting.Rows];

                for (int i = 1; i < GridSetting.Rows + 1; i++)
                {
                    E[i - 1] = "same";
                }

                return E;
            }

        static int DefaultSequence_Confirmed = 1;

        static List<string[]> Sequence = new List<string[]>();

        //Convert Element Number Direction to Number
        static int TranslateToElementNumber(int Layer, int i, int j, string MatchElementNum)
            {

                if (i == -1) { return -1; }
                if (MatchElementNum == "same")
                {
                    return BuildGrid.Layer[Layer].Spaces[i, j].Elementnum;
                }
                else
                {
                    int number;
                    bool success = int.TryParse(MatchElementNum, out number);
                    if (success)
                    {
                        return number;
                    }
                    else
                    {
                        return -1;
                    }

                }

            }

        //Find Matches based on Assigned strings
        static void FindMatch(int Layer, int i, int j, Vector2Int[] MatchDirection, string[] MatchElementNum,int ConfirmNumber, out bool MatchConditionMet, out Vector2Int[] SpacesChecked)
        {
                int[,] ElementNums = BuildGrid.Layer[Layer].AllElementsNumbersInLayer();
                MatchConditionMet = false;
                SpacesChecked = null;
                int SpacesCheckedCount = 1;
                Vector2Int[] tempSpacesChecked = new Vector2Int[GridSetting.Columns * GridSetting.Rows];


                //Initial Position
                Vector2Int Firstpos = new Vector2Int(i, j);
                Vector2Int pos = new Vector2Int(i, j);
                Vector2Int Nextpos = new Vector2Int(-1, -1);
                int NextExpectedElementNum = -1;

                tempSpacesChecked[0] = new Vector2Int(i, j);

                //Run while you find the matches
                for (int r = 0; r < MatchDirection.Length+1; r++)
                {
                 
                    //Confirm Match at this point
                    if (r >= MatchDirection.Length)
                    {

                        MatchConditionMet = true;
                        Vector2Int[] SpacesCheckedFinal = new Vector2Int[SpacesCheckedCount];
                        for (int c = 0; c < SpacesCheckedCount; c++) { SpacesCheckedFinal[c] = tempSpacesChecked[c]; }
                        SpacesChecked = SpacesCheckedFinal;

                        break; //Reached the end of matches to find
                    

                    }
                    else
                    {

                        Nextpos = GridSetting.SpaceNextToPos(i, j, MatchDirection[r].x, 0, 0, MatchDirection[r].y);

                        if (MatchElementNum[r] == "same") { NextExpectedElementNum = BuildGrid.Layer[Layer].Spaces[i, j].Elementnum; }
                        else { NextExpectedElementNum = TranslateToElementNumber(Layer, pos.x, pos.y, MatchElementNum[r]); }
                    }

                    //Next Space Exist and is the Same Element Number
                    if (Nextpos.x != -1 && ElementNums[Nextpos.x, Nextpos.y] == NextExpectedElementNum)
                    {
                        tempSpacesChecked[SpacesCheckedCount] = Nextpos;
                        SpacesCheckedCount++;
                        pos = Nextpos; //Set to next Position

                        NextExpectedElementNum = -1;
                        if (ConfirmNumber == r) { MatchConditionMet = true; }

                    }
                    else
                    {

                        Vector2Int[] SpacesCheckedFinal = new Vector2Int[SpacesCheckedCount];
                        for (int c = 0; c < SpacesCheckedCount; c++) { SpacesCheckedFinal[c] = tempSpacesChecked[c]; }
                        SpacesChecked = SpacesCheckedFinal;

                        break; 
                    }

                }
        }

         //Return Element Matches
        static void GetElementMatches(int Layer, string Name = "default", string[] Names = null)
        {
                List<Vector2Int[]> AllMatchedPositions = new List<Vector2Int[]>();
                bool[,] PositionChecked = new bool[GridSetting.Rows, GridSetting.Columns];
                int TotalMatchesFound = 0;

                //Run through each position
                for (int i = 0; i < GridSetting.Rows; i++)
                {
                    for (int j = 0; j < GridSetting.Columns; j++)
                    {

                        //Position is not Checked and not Empty
                        if (PositionChecked[i, j] == false && BuildGrid.Layer[Layer].Spaces[i, j].Exist() && BuildGrid.Layer[Layer].Spaces[i, j].IsMatchable)
                        {

                            if (Name == "default" && Names == null)
                            {
                                bool MatchConditionMet = false;
                                Vector2Int[] MatchesFound = null;
                                bool MatchConditionMet2 = false;
                                Vector2Int[] MatchesFound2 = null;

                                //Find Matches from Position Based on Match Order
                               FindMatch(Layer, i, j, DefaultSeq1(), DefaultElements1(), DefaultSequence_Confirmed, out MatchConditionMet, out MatchesFound);
                               FindMatch(Layer, i, j, DefaultSeq2(), DefaultElements2(), DefaultSequence_Confirmed, out MatchConditionMet2, out MatchesFound2);


                                if (MatchConditionMet && MatchesFound != null)
                                {

                                        PositionsMatched.Add(MatchesFound);
                                        NamesMatched.Add("default1");
                                        TotalMatchesFound++;
                                }
                                if (MatchConditionMet2 && MatchesFound2 != null)
                                {
                                        PositionsMatched.Add(MatchesFound2);
                                        NamesMatched.Add("default2");
                                        TotalMatchesFound++;
                                }


                            }
                            else
                            {


                                for (int count = 0; count < Sequences.Count; count++)
                                {
                                    bool MatchConditionMet = false;
                                    Vector2Int[] MatchesFound = null;

                                    if (Names == null && Sequences[count].Name == Name)
                                    {

                                        FindMatch(Layer, i, j, Sequences[count].Sequence, Sequences[count].Sequence_ElementNumber, Sequences[count].Sequence_Confirmed, out MatchConditionMet, out MatchesFound);
                                    }
                                    else if(Names != null )
                                    {
                                        
                                        //If string name is found in array of names
                                        foreach(string s in Names)
                                        {
                                            if(s == Sequences[count].Name)
                                            {
                                                FindMatch(Layer, i, j, Sequences[count].Sequence, Sequences[count].Sequence_ElementNumber, Sequences[count].Sequence_Confirmed, out MatchConditionMet, out MatchesFound);
                                            }
                                        }
                                        
                                    }

                                  

                                    if (MatchConditionMet && MatchesFound[count] != null)
                                    {
                                        PositionsMatched.Add(MatchesFound);
                                        NamesMatched.Add(Sequences[count].Name);
                                        TotalMatchesFound++;
  
                                    }
                                    

                                }



                            }

                            PositionChecked[i, j] = true;
                        }

                    }
                }

            }

        /// <summary>Get list of Matches</summary>
        private static void Match(int Layer, bool DestroyMatches, float DestroyTimer, string Name = "default", string[] Names = null)
        {

            float T = Time.time;
            GetElementMatches(Layer, Name, Names);

            if (DestroyMatches)
            {
                for (int count = 0; count < PositionsMatched.Count; count++)
                {
                    foreach (Vector2Int Pos in PositionsMatched[count])
                    {
                        if (Pos.x != -1)
                        {

                            BuildGrid.Layer[Layer].DeleteElementAtSpace(Pos.x, Pos.y, DestroyTimer);


                        }
                    }
                }
            }
        }

        static List<Vector2Int[]> PositionsMatched = new List<Vector2Int[]>();
        static List<string> NamesMatched = new List<string>();
        #endregion

        #region User References

        /// <summary>Add a Custom Match Pattern.</summary>
        /// <param name="Name">Set the Name of the Match Pattern.</param>
        /// <param name="Sequence">An Array of Positions to Set the Direction of the Match Path.</param>
        /// <param name="Sequence_ElementNumber">An Array of Specific Strings to Set the Expected Element Numbers of the Match Path.</param>
        /// <param name="SequenceConfirmedAtArray">The Array Position which represents the Minimum Criteria to be a Complete Match Pattern.</param>
        public static void SetCustomMatch(string Name, Vector2Int[] Sequence, string[] Sequence_ElementNumber, int SequenceConfirmedAtArray)
        {
            bool Sequence_OK = false;
            bool Sequence_ElementNumber_OK = false;

            //Check Sequence for error
            foreach (Vector2Int s in Sequence)
            {
                if(s.x != -1 && s.y != -1)
                {
                    Sequence_OK = true;
                }
                else
                {
                    break;
                }
            }

            //Check Element Number Sequence for error
            foreach (string s in Sequence_ElementNumber)
            {
                int number;
                bool success = int.TryParse(s, out number);
                if (success || s == "same")
                {
                    Sequence_ElementNumber_OK = true;
                }
                else
                {
                    break;
                }
             
                
            }

            //Check if Conditions are Met
            if(Sequence_OK && Sequence_ElementNumber_OK)
            {

                Sequences.Add(new MatchSet());
                Sequences[Sequence_Count].Name = Name;
                Sequences[Sequence_Count].Sequence = Sequence;
                Sequences[Sequence_Count].Sequence_ElementNumber = Sequence_ElementNumber;
                Sequences[Sequence_Count].Sequence_Confirmed = SequenceConfirmedAtArray;

                Sequence_Count++;
               // MatchSet.Sequence_Confirmed.Add(Name, SequenceConfirmedAtArray);
               //MatchSet.Sequence.Add(Name, Sequence);
               // MatchSet.Sequence_ElementNumber.Add(Name, Sequence_ElementNumber);

            }
            else
            {
                Debug.LogError("Match3 Creator //BuildMatching: Sequence is Invalid *Read Readme for more Information");
            }
        }

        /// <summary>A Method that Outputs a List of Matches and their Names. The Default Match Pattern is 3 or more of the same element in a row or column, however this can be changed using SetCustomMatch().</summary>
        /// <param name="Layer">The Layer to Look for Matches.</param>
        /// <param name="Positions">Output of a List of Grid Row and Column Positions that are in a Match Pattern.</param>
        /// <param name="MatchNames">Output of a List of Names associated with the List of Positions.</param>
        /// <param name="Name">Sets the Match Pattern to Look for; "default" is the built-in Match Pattern; Any other Match Pattern will need to be set by SetCustomMatch.</param>
        /// <param name="Names">Sets Multiple types of Match Pattern to Look for; Will INGORE "Name" parameter if set.</param>
        public static void GetMatches(int Layer,out List<Vector2Int[]> Positions, out List<string> MatchNames, string Name = "default", string[] Names = null)
        {
            if (!BuildMove.Fall.FallingActive() && !BuildSwap.Preset.AnySwapEngaged())
            {
                PositionsMatched = null; PositionsMatched = new List<Vector2Int[]>();
                NamesMatched = null; NamesMatched = new List<string>();

                Match(Layer, false, 0, Name, Names);

                Positions = PositionsMatched;
                MatchNames = NamesMatched;
            }
            else
            {
                Debug.LogWarning("Match3 Creator : BuildMatch - Cannot Match While Falling or Swapping");
                Positions = null;
                MatchNames = null;
            }
        }

        #endregion


    }
}